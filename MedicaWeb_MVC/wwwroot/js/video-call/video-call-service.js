const servers = {
  iceServers: [
    { urls: "stun:stun.l.google.com:19302" },
    { urls: "stun:stun.l.google.com:5349" },
    { urls: "stun:stun1.l.google.com:3478" },
    { urls: "stun:stun1.l.google.com:5349" },
    { urls: "stun:stun2.l.google.com:19302" },
    { urls: "stun:stun2.l.google.com:5349" },
    { urls: "stun:stun3.l.google.com:3478" },
    { urls: "stun:stun3.l.google.com:5349" },
    { urls: "stun:stun4.l.google.com:19302" },
    { urls: "stun:stun4.l.google.com:5349" },
    //  {
    //    urls: "stun:stun.relay.metered.ca:80",
    //  },
    //  {
    //    urls: "turn:global.relay.metered.ca:80",
    //    username: "c25b233a28eae1c1638e1e1a",
    //    credential: "TJEsJh/jX/7rgRFB",
    //  },
    //  {
    //    urls: "turn:global.relay.metered.ca:80?transport=tcp",
    //    username: "c25b233a28eae1c1638e1e1a",
    //    credential: "TJEsJh/jX/7rgRFB",
    //  },
    //  {
    //    urls: "turn:global.relay.metered.ca:443",
    //    username: "c25b233a28eae1c1638e1e1a",
    //    credential: "TJEsJh/jX/7rgRFB",
    //  },
    //  {
    //    urls: "turns:global.relay.metered.ca:443?transport=tcp",
    //    username: "c25b233a28eae1c1638e1e1a",
    //    credential: "TJEsJh/jX/7rgRFB",
    //  },
  ],
};
export class VideoCallService {
  constructor(userRole, userName) {
    this.userRole = userRole;
    this.userName = userName;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/WebRTCHub")
      .withAutomaticReconnect()
      .build();
    this.localStream = null;
    this.remoteStreams = new Map(); // Store multiple remote streams
    this.connectionId = null;
    this.peerConnections = new Map();
    this.onRemoteStreamCallback = null;
    this.onConnectionIdCallback = null;
    this.onUserLeftCallback = null;
  }

  async setCallbacks(callbacks) {
    this.onRemoteStreamCallback = callbacks.onRemoteStreamCallback;
    if (callbacks.onConnectionIdCallback) {
      this.onConnectionIdCallback = callbacks.onConnectionIdCallback;
      this.onConnectionIdCallback(this.connectionId);
    }
    if (callbacks.onUserLeftCallback) {
      this.onUserLeftCallback = callbacks.onUserLeftCallback;
    }
  }

  async startConnection() {
    this.connection.onreconnecting((error) => {
      console.log("SignalR connection lost. Attempting to reconnect...", error);
      this.connectionId = null;
      if (this.onConnectionIdCallback) {
        this.onConnectionIdCallback(null);
      }
    });

    this.connection.onreconnected((connectionId) => {
      console.log("SignalR connection reestablished.", connectionId);
      this.connectionId = connectionId;
      if (this.onConnectionIdCallback) {
        this.onConnectionIdCallback(connectionId);
      }
      // Rejoin room if we were in one
      if (this.roomId) {
        this.joinRoom(this.roomId);
      }
    });

    this.connection.onclose((error) => {
      console.log("SignalR connection closed.", error);
      this.connectionId = null;
      if (this.onConnectionIdCallback) {
        this.onConnectionIdCallback(null);
      }
      // Clean up peer connections
      this.peerConnections.forEach((connection, connectionId) => {
        connection.close();
        this.peerConnections.delete(connectionId);
      });
      // Clear streams
      if (this.localStream) {
        this.localStream.getTracks().forEach((track) => track.stop());
        this.localStream = null;
      }
      this.remoteStreams.forEach((stream) => {
        stream.getTracks().forEach((track) => track.stop());
      });
      this.remoteStreams.clear();
      if (this.onRemoteStreamCallback) {
        this.onRemoteStreamCallback(null);
      }

      // Don't try to invoke methods when the connection is closed
      console.log("Connection closed, room:", this.roomId);
      this.roomId = null;
    });

    try {
      await this.connection.start();
      console.log("SignalR connection started successfully");

      // Get the connection ID after successful connection
      this.connectionId = this.connection.connectionId;

      // Set up handlers after successful connection
      this.handleUserJoined();
      this.handleReceiveOffer();
      this.handleReceiveAnswer();
      this.handleReceiveIceCandidate();
      this.handleUserLeft();

      return true;
    } catch (error) {
      console.error("Error starting SignalR connection:", error);
      throw error;
    }
  }

  stopConnection() {
    this.connection.stop();
  }

  setLocalStream(stream) {
    this.localStream = stream;
  }

  async leaveRoom() {
    if (this.connection.state === "Connected") {
      await this.connection.invoke("LeaveRoom", this.roomId);
      console.log("Left room:", this.roomId);
    } else {
      console.log(
        `Cannot leave room: connection state is ${this.connection.state}`
      );
    }
  }

  async joinRoom(roomId) {
    try {
      if (this.connection.state === "Connected") {
        await this.connection.invoke("JoinRoom", roomId);
        this.roomId = roomId;
        console.log("Joined room:", roomId);
      } else {
        console.error(
          `Cannot join room: connection state is ${this.connection.state}`
        );
        throw new Error(`Cannot join room: connection is not connected`);
      }
    } catch (error) {
      console.error("Error joining room:", error);
      throw error;
    }
  }

  handleUserJoined() {
    this.connection.on("UserJoined", (connectionId, username, userRole) => {
      if (!this.peerConnections.has(connectionId)) {
        console.log("Creating peer connection for:", connectionId);
        this.createPeerConnection(connectionId, username, userRole);
      }
      console.log("User joined:", connectionId, username);
    });
  }

  async sendOfferToUser(connectionId, username, userRole) {
    const peerConnection = this.peerConnections.get(connectionId);

    if (!peerConnection) {
      console.warn(`No peer connection found for ${connectionId}`);
      return;
    }

    try {
      // 1. Verify signaling state
      if (peerConnection.signalingState !== "stable") {
        console.log(
          `Waiting for stable state. Current state: ${peerConnection.signalingState}`
        );
        await new Promise((resolve) => {
          const checkState = () => {
            if (!peerConnection || peerConnection.signalingState === "stable") {
              resolve();
            } else {
              setTimeout(checkState, 500);
            }
          };
          checkState();
        });
      }

      // 2. Skip if we already have a local offer
      if (peerConnection.localDescription?.type === "offer") {
        console.log(`Already have local offer for ${connectionId}, skipping`);
        return;
      }

      // 3. Create offer based on role
      const offerOptions = {
        offerToReceiveAudio: true,
        offerToReceiveVideo: this.userRole === "Student",
      };

      console.log(`Creating offer with options:`, offerOptions);
      const offer = await peerConnection.createOffer(offerOptions);

      // 4. Set local description
      await peerConnection.setLocalDescription(offer);
      console.log(`Local description set for ${connectionId}`);

      // 5. Send offer if connection is active
      if (
        this.connection.state === "Connected" &&
        peerConnection.localDescription
      ) {
        await this.connection.invoke(
          "SendOffer",
          this.roomId,
          JSON.stringify(peerConnection.localDescription),
          this.userRole,
          this.userName
        );
        console.log(`Offer sent to ${connectionId}`);
      }
    } catch (error) {
      console.error("Error in sendOfferToUser:", error);

      if (
        error.message.includes("m-lines") ||
        error.message.includes("Failed to set")
      ) {
        await this.handleNegotiationError(connectionId, username, userRole);
      }
    }
  }

  async handleNegotiationError(connectionId, username, userRole) {
    console.log("Handling negotiation error for", connectionId);

    // 1. Close existing connection
    const oldConnection = this.peerConnections.get(connectionId);
    if (oldConnection) {
      oldConnection.close();
      this.peerConnections.delete(connectionId);
    }

    // 2. Clean up existing streams
    const oldStream = this.remoteStreams.get(connectionId);
    if (oldStream) {
      oldStream.getTracks().forEach((track) => track.stop());
      this.remoteStreams.delete(connectionId);
    }

    // 3. Create new connection with proper configuration
    await this.createPeerConnection(connectionId, username, userRole);
  }

  createPeerConnection(connectionId, username, userRole) {
    // Close existing connection if any
    if (this.peerConnections.has(connectionId)) {
      const existing = this.peerConnections.get(connectionId);
      existing?.close();
      this.peerConnections.delete(connectionId);

      const existingStream = this.remoteStreams.get(connectionId);
      if (existingStream) {
        existingStream.getTracks().forEach((track) => track.stop());
        this.remoteStreams.delete(connectionId);
      }
    }

    // Create new connection
    const peerConnection = new RTCPeerConnection(servers);
    this.peerConnections.set(connectionId, peerConnection);

    // Create new MediaStream
    const newStream = new MediaStream();
    this.remoteStreams.set(connectionId, newStream);

    // Set up transceivers with consistent ordering
    this.setupTransceivers(peerConnection, userRole);

    // Add tracks if we have a local stream
    if (this.localStream) {
      this.addLocalTracks(peerConnection);
    }

    // Set up event handlers
    this.setupPeerConnectionHandlers(
      peerConnection,
      connectionId,
      username,
      userRole
    );

    return peerConnection;
  }

  setupTransceivers(peerConnection, userRole) {
    console.log(
      `Setting up transceivers for local role: ${this.userRole}, peer role: ${userRole}`
    );

    // 1. Video transceiver - handle based on roles
    let videoDirection;
    if (this.userRole === "Lecturer") {
      videoDirection = "sendonly"; // Lecturer sends video
    } else if (userRole === "Lecturer") {
      videoDirection = "recvonly"; // Students receive lecturer's video
    } else {
      videoDirection = "inactive"; // No video between students
    }

    const videoTransceiver = peerConnection.addTransceiver("video", {
      direction: videoDirection,
    });

    // 2. Audio transceiver - everyone can talk
    const audioTransceiver = peerConnection.addTransceiver("audio", {
      direction: "sendrecv",
    });

    console.log(`Transceivers created:`, {
      video: videoDirection,
      audio: "sendrecv",
      videoTrack: videoTransceiver.sender.track?.id || "none",
      audioTrack: audioTransceiver.sender.track?.id || "none",
    });
  }

  async addLocalTracks(peerConnection) {
    if (!this.localStream) {
      console.warn("No local stream available for adding tracks");
      return;
    }

    try {
      const videoTracks = this.localStream.getVideoTracks();
      const audioTracks = this.localStream.getAudioTracks();

      console.log("Available local tracks:", {
        video: videoTracks.length,
        audio: audioTracks.length,
      });
      if (videoTracks.length > 0) {
        videoTracks.forEach((track) => {
          console.log(`Adding video track: ${track.id} to peer connection`);
          peerConnection.addTrack(track, this.localStream);
        });
      }

      if (audioTracks.length > 0) {
        audioTracks.forEach((track) => {
          console.log(`Adding audio track: ${track.id} to peer connection`);
          peerConnection.addTrack(track, this.localStream);
        });
      }

    //   // Get existing transceivers
    //   const transceivers = peerConnection.getTransceivers();
    //   const videoTransceiver = transceivers.find(
    //     (t) => t.sender.track?.kind === "video" || t.mid === "0"
    //   );
    //   const audioTransceiver = transceivers.find(
    //     (t) => t.sender.track?.kind === "audio" || t.mid === "1"
    //   );

    //   // Add video tracks if we're a lecturer
    //   if (
    //     this.userRole === "Lecturer" &&
    //     videoTracks.length > 0 &&
    //     videoTransceiver
    //   ) {
    //     console.log("Adding video track for lecturer");
    //     try {
    //       await videoTransceiver.sender.replaceTrack(videoTracks[0]);
    //       console.log("Video track added successfully");
    //     } catch (error) {
    //       console.error("Failed to add video track:", error);
    //     }
    //   }

    //   // Add audio tracks for everyone
    //   if (audioTracks.length > 0 && audioTransceiver) {
    //     console.log("Adding audio track");
    //     try {
    //       await audioTransceiver.sender.replaceTrack(audioTracks[0]);
    //       console.log("Audio track added successfully");
    //     } catch (error) {
    //       console.error("Failed to add audio track:", error);
    //     }
    //   }

    //   // Verify track attachment
    //   this.verifyTrackAttachment(peerConnection);
    } catch (error) {
      console.error("Error in addLocalTracks:", error);
    }
  }

  verifyTrackAttachment(peerConnection) {
    const senders = peerConnection.getSenders();
    const videoSender = senders.find((s) => s.track?.kind === "video");
    const audioSender = senders.find((s) => s.track?.kind === "audio");

    console.log("Track attachment verification:", {
      video: videoSender
        ? {
            track: videoSender.track?.id || "none",
            enabled: videoSender.track?.enabled || false,
            muted: videoSender.track?.muted || true,
          }
        : "no sender",
      audio: audioSender
        ? {
            track: audioSender.track?.id || "none",
            enabled: audioSender.track?.enabled || false,
            muted: audioSender.track?.muted || true,
          }
        : "no sender",
    });
  }

  setupPeerConnectionHandlers(
    peerConnection,
    connectionId,
    username,
    userRole
  ) {
    peerConnection.ontrack = (event) => {
      console.log(`Track received from ${username}:`, {
        kind: event.track.kind,
        id: event.track.id,
        enabled: event.track.enabled,
      });

      if (event.streams && event.streams[0]) {
        const stream = event.streams[0];
        this.remoteStreams.set(connectionId, stream);

        // Monitor track states
        event.track.onmute = () => console.log(`Track ${event.track.id} muted`);
        event.track.onunmute = () =>
          console.log(`Track ${event.track.id} unmuted`);
        event.track.onended = () =>
          console.log(`Track ${event.track.id} ended`);

        this.handleUpdateRemoteDisplay(connectionId, username, userRole);
      } else {
        console.warn(`Received track without stream from ${username}`);
      }
    };

    peerConnection.onicecandidate = (event) => {
      if (event.candidate && this.connection.state === "Connected") {
        this.connection
          .invoke(
            "SendIceCandidate",
            connectionId,
            JSON.stringify(event.candidate)
          )
          .catch((error) => {
            console.error("Error sending ICE candidate:", error);
          });
      }
    };

    peerConnection.onnegotiationneeded = async () => {
      console.log(`Negotiation needed for ${connectionId}`);
      if (peerConnection.signalingState === "stable") {
        if (this.connectionId && this.connectionId > connectionId) {
          this.sendOfferToUser(connectionId, username, userRole);
        }
      }
    };

    // Add connection state monitoring
    this.setupConnectionStateMonitoring(peerConnection, connectionId);
  }

  setupConnectionStateMonitoring(peerConnection, connectionId) {
    peerConnection.onconnectionstatechange = () => {
      console.log(
        `Connection state for ${connectionId}:`,
        peerConnection.connectionState
      );

      switch (peerConnection.connectionState) {
        case "failed":
          this.handleConnectionFailure(connectionId);
          break;
        case "disconnected":
          setTimeout(() => this.handleDisconnection(connectionId), 2000);
          break;
      }
    };
  }

  async handleConnectionFailure(connectionId) {
    console.log(`Handling connection failure for ${connectionId}`);

    const peerConnection = this.peerConnections.get(connectionId);
    if (peerConnection?.restartIce) {
      try {
        await peerConnection.restartIce();
        console.log(`ICE restart initiated for ${connectionId}`);
      } catch (error) {
        console.error("Error restarting ICE:", error);
        await this.handleNegotiationError(connectionId);
      }
    }
  }

  async handleDisconnection(connectionId) {
    const peerConnection = this.peerConnections.get(connectionId);
    if (peerConnection?.connectionState === "disconnected") {
      console.log(
        `Attempting to recover disconnected connection: ${connectionId}`
      );
      await this.handleConnectionFailure(connectionId);
    }
  }

  handleReceiveAnswer() {
    console.log("Handling receive answer");
    this.connection.on("ReceiveAnswer", async (answer, fromConnectionId) => {
      console.log("Received answer from user:", fromConnectionId);

      const peerConnection = this.peerConnections.get(fromConnectionId);

      if (peerConnection) {
        try {
          const parsedAnswer = JSON.parse(answer);

          // Check if we're in the right state to receive an answer
          if (peerConnection.signalingState !== "have-local-offer") {
            console.warn(
              `Cannot set remote answer: signaling state is ${peerConnection.signalingState}, expected 'have-local-offer'`
            );
            return;
          }

          await peerConnection.setRemoteDescription(
            new RTCSessionDescription(parsedAnswer)
          );

          console.log("Answer processed from user:", fromConnectionId);
        } catch (error) {
          console.error("Error processing answer:", error);

          // If we have an invalid state error, the negotiation may be out of sync
          if (
            error instanceof Error &&
            (error.message.includes("Called in wrong state") ||
              error.message.includes("Failed to set remote answer sdp"))
          ) {
            console.warn("Invalid state for answer, restarting negotiation...");

            // Restart the negotiation process
            if (this.connectionId && this.connectionId > fromConnectionId) {
              setTimeout(() => {
                this.sendOfferToUser(fromConnectionId);
              }, 1000);
            }
          }
        }
      } else {
        console.warn(`No peer connection found for ${fromConnectionId}`);
      }
    });
  }

  handleReceiveOffer() {
    this.connection.on(
      "ReceiveOffer",
      async (offer, fromConnectionId, offerUsername, offerRole) => {
        try {
          console.log(`Received offer from ${fromConnectionId}`);

          // Parse the offer
          let parsedOffer;
          try {
            parsedOffer = JSON.parse(offer);
            console.log("Parsed offer:", parsedOffer);
          } catch (error) {
            console.error("Error parsing offer:", error);
            return;
          }

          // Get or create peer connection
          let peerConnection = this.peerConnections.get(fromConnectionId);
          if (!peerConnection) {
            console.log(`Creating new peer connection for ${fromConnectionId}`);
            this.createPeerConnection(
              fromConnectionId,
              offerUsername,
              offerRole
            );
            peerConnection = this.peerConnections.get(fromConnectionId);
          }

          // Handle different signaling states properly
          if (peerConnection.signalingState !== "stable") {
            console.log(
              `Peer connection not in stable state (${peerConnection.signalingState}), rolling back...`
            );

            // If we have a pending local offer, roll it back
            if (peerConnection.signalingState === "have-local-offer") {
              await peerConnection.setLocalDescription({ type: "rollback" });
              console.log("Rolled back local description");
            }

            // Wait for rollback to complete
            await new Promise((resolve) => {
              const checkState = () => {
                if (
                  !peerConnection ||
                  peerConnection.signalingState === "stable"
                ) {
                  resolve();
                } else {
                  setTimeout(checkState, 100);
                }
              };
              checkState();
            });

            console.log(
              "Signaling state is now stable, continuing with offer processing"
            );
          }

          // Set the remote description
          await peerConnection.setRemoteDescription(
            new RTCSessionDescription(parsedOffer)
          );
          console.log(`Remote description set for ${fromConnectionId}`);

          // Create and set local answer
          const answer = await peerConnection.createAnswer();

          // Check signaling state before setting local description
          if (peerConnection.signalingState === "have-remote-offer") {
            await peerConnection.setLocalDescription(answer);
            console.log(
              `Local description (answer) set for ${fromConnectionId}`
            );

            // Send the answer
            if (this.connection.state === "Connected") {
              await this.connection.invoke(
                "SendAnswer",
                fromConnectionId,
                JSON.stringify(answer)
              );
              console.log(`Answer sent to ${fromConnectionId}`);
            } else {
              console.error(
                `Cannot send answer: connection state is ${this.connection.state}`
              );
            }
          } else {
            console.warn(
              `Cannot set local description: Signaling state is ${peerConnection.signalingState}, expected 'have-remote-offer'`
            );
          }
        } catch (error) {
          console.error("Error handling offer:", error);

          // If we have an SDP-related error, recreate the connection
          if (
            error instanceof Error &&
            (error.message.includes(
              "The order of m-lines in subsequent offer doesn't match"
            ) ||
              error.message.includes("Called in wrong state") ||
              error.message.includes("Failed to set remote offer sdp"))
          ) {
            console.log("SDP error detected. Recreating peer connection...");

            // Get the existing connection
            const peerConnection = this.peerConnections.get(fromConnectionId);
            if (peerConnection) {
              peerConnection.close();
              this.peerConnections.delete(fromConnectionId);
            }

            // Create a new connection
            this.createPeerConnection(
              fromConnectionId,
              offerUsername,
              offerRole
            );

            // Try processing the offer again after a short delay
            //  setTimeout(() => {
            //    if (this.connection.state === "Connected") {
            //      this.handleProcessOffer(offer, fromConnectionId);
            //    }
            //  }, 1000);
          }
        }
      }
    );
  }

  async handleProcessOffer(offer, fromConnectionId) {
    try {
      console.log(`Reprocessing offer from ${fromConnectionId}`);

      const parsedOffer = JSON.parse(offer);
      const peerConnection = this.peerConnections.get(fromConnectionId);

      if (!peerConnection) {
        console.warn(`No peer connection found for ${fromConnectionId}`);
        return;
      }

      // Set the remote description
      await peerConnection.setRemoteDescription(
        new RTCSessionDescription(parsedOffer)
      );

      // Create and set local answer
      const answer = await peerConnection.createAnswer();
      await peerConnection.setLocalDescription(answer);

      // Send the answer
      if (this.connection.state === "Connected") {
        await this.connection.invoke(
          "SendAnswer",
          fromConnectionId,
          JSON.stringify(answer)
        );
      }
    } catch (error) {
      console.error("Error reprocessing offer:", error);
    }
  }

  handleReceiveIceCandidate() {
    this.connection.on(
      "ReceiveIceCandidate",
      async (iceCandidate, fromConnectionId) => {
        const peerConnection = this.peerConnections.get(fromConnectionId);

        if (peerConnection) {
          try {
            console.log("Adding ice candidate:", iceCandidate);
            await peerConnection.addIceCandidate(
              new RTCIceCandidate(JSON.parse(iceCandidate))
            );
          } catch (error) {
            console.error("Error adding ice candidate:", error);
          }
        } else {
          console.warn(`No peer connection found for ${fromConnectionId}`);
        }
      }
    );
  }

  handleUserLeft() {
    console.log("Handling user left");
    this.connection.on("UserLeft", (connectionId, username) => {
      if (this.peerConnections.has(connectionId)) {
        this.peerConnections.get(connectionId)?.close();
        this.peerConnections.delete(connectionId);
        this.remoteStreams.delete(connectionId);
        if (this.onUserLeftCallback) {
          this.onUserLeftCallback(username);
        }
        console.log("User left:", connectionId);
      }
    });
  }

  handleUpdateRemoteDisplay(connectionId, username, userRole) {
    const remoteStream = this.remoteStreams.get(connectionId);
    if (!remoteStream) {
      console.warn(`No remote stream found for ${connectionId}`);
      return;
    }

    console.log(`Updating remote display for ${username}:`, {
      videoTracks: remoteStream.getVideoTracks().length,
      audioTracks: remoteStream.getAudioTracks().length,
    });

    // Log track states
    remoteStream.getTracks().forEach((track) => {
      console.log(`Remote track: ${track.kind}`, {
        id: track.id,
        enabled: track.enabled,
        muted: track.muted,
        readyState: track.readyState,
      });
    });

    if (this.onRemoteStreamCallback) {
      this.onRemoteStreamCallback(remoteStream, username, userRole);
    }
  }

  getConnectionState() {
    return this.connection?.state || "Disconnected";
  }

  getCurrentRoom() {
    return this.roomId;
  }

  getConnectionId() {
    return this.connectionId;
  }
}

document.addEventListener(
  "DOMContentLoaded",
  () => {
    // initialization code
  },
  { once: true }
); // ensure it only runs once
