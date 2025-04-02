import { VideoCallService } from "./video-call-service.js";

const userRole = document.getElementById("userRole");
const userName = document.getElementById("userName");
const fullName = document.getElementById("fullName");
const participantItems = document.getElementById("participantItems");
const videoCallService = new VideoCallService(userRole.value, userName.value);
const roomId = "123";

// if userRole is lecturer, then this is considered as local video
const lecturerVideoRef = document.getElementById("lecturer-video");
// if userRole is student, then this is considered as remote video
const localVideoRef =
  userRole.value === "Lecturer"
    ? lecturerVideoRef
    : document.getElementById(`participant-video`);

// const remoteVideoRef = document.getElementById("remote-video");
const participantVideoRefs = new Map();

// Get initial state from window object
const state = window.videoCallState || {
  isVideoEnabled: true,
  isAudioEnabled: true,
  isSignalRConnected: true,
  isWebcamActive: false,
};

// Create state management functions
const updateState = (updates) => {
  Object.assign(state, updates);
  window.videoCallState = state; // Keep window object in sync
  // Trigger UI updates if needed
  updateUI();
};

const updateUI = () => {
  // Update UI based on state
  const videoBtn = document.getElementById("videoBtn");
  const audioBtn = document.getElementById("audioBtn");

  if (videoBtn) {
    videoBtn.classList.toggle("video-off", !state.isVideoEnabled);
    videoBtn.innerHTML = state.isVideoEnabled
      ? '<i class="bi bi-camera-video-fill"></i>'
      : '<i class="bi bi-camera-video-off-fill"></i>';
  }

  if (audioBtn) {
    audioBtn.classList.toggle("muted", !state.isAudioEnabled);
    audioBtn.innerHTML = state.isAudioEnabled
      ? '<i class="bi bi-mic-fill"></i>'
      : '<i class="bi bi-mic-mute-fill"></i>';
  }
};

// Add function to get webcam state
window.getWebcamState = () => state.isWebcamActive;

// Modified toggle functions
function toggleVideo() {
  updateState({ isVideoEnabled: !state.isVideoEnabled });
  if (localVideoRef?.srcObject) {
    const videoTrack = localVideoRef.srcObject.getVideoTracks()[0];
    if (videoTrack) {
      videoTrack.enabled = state.isVideoEnabled;
    }
  }
}

function toggleAudio() {
  updateState({ isAudioEnabled: !state.isAudioEnabled });
  if (localVideoRef?.srcObject) {
    const audioTrack = localVideoRef.srcObject.getAudioTracks()[0];
    if (audioTrack) {
      audioTrack.enabled = state.isAudioEnabled;
    }
  }
}

// Make toggle functions globally available
window.toggleVideo = toggleVideo;
window.toggleAudio = toggleAudio;

function addParticipantVideo(userName, remoteUserRole, stream) {
  if (!stream) {
    console.warn("No stream provided for participant:", userName);
    return;
  }

  // Handle lecturer's stream
  if (remoteUserRole === "Lecturer") {
    lecturerVideoRef.srcObject = stream;
    lecturerVideoRef.muted = false;
    lecturerVideoRef.volume = 1;
  } else {
    // Handle student's stream
    const video = document.createElement("video");
    video.autoplay = true;
    video.playsInline = true;
    video.muted = false;
    video.volume = 1;
    video.srcObject = stream;
    video.id = `video-${userName}`;
    video.style.display = "none"; // Hide video element but keep it for audio
    document.body.appendChild(video);
    participantVideoRefs.set(userName, video);
  }

  // Create participant list item if not lecturer viewing student
  const participantItem = document.createElement("div");
  participantItem.className = "participant-item active";
  participantItem.id = `participant-${userName}`;

  // Add CSS styling
  participantItem.style.background = "rgba(255, 255, 255, 0.2)";
  participantItem.style.borderRadius = "8px";
  participantItem.style.padding = "0.8rem";
  participantItem.style.marginBottom = "0.8rem";
  participantItem.style.transition = "all 0.3s ease";
  participantItem.style.border = "2px solid transparent"; // Add transparent border by default

  const participantInfo = document.createElement("div");
  participantInfo.className = "participant-info";
  participantInfo.style.display = "flex";
  participantInfo.style.justifyContent = "space-between";
  participantInfo.style.alignItems = "center";
  participantInfo.style.color = "white";

  const nameSpan = document.createElement("span");
  nameSpan.className = "name";
  nameSpan.textContent =
    remoteUserRole === "Lecturer" ? "Lecturer" : `Student ${userName}`;
  nameSpan.style.fontSize = "0.9rem";

  const statusSpan = document.createElement("span");
  statusSpan.className = "status";
  statusSpan.textContent = "Connected";
  statusSpan.style.fontSize = "0.8rem";
  statusSpan.style.color = "rgba(255, 255, 255, 0.7)";

  participantInfo.appendChild(nameSpan);
  participantInfo.appendChild(statusSpan);
  participantItem.appendChild(participantInfo);
  participantItems.appendChild(participantItem);

  // Add audio monitoring for the stream
  monitorAudioLevel(stream, userName);
}

function removeParticipantVideo(username) {
  // Remove from participant list
  const participantItem = document.getElementById(`participant-${username}`);
  if (participantItem) {
    participantItem.remove();
  }

  // Remove hidden video element
  const video = document.getElementById(`video-${username}`);
  if (video) {
    video.remove();
  }

  participantCount--;
}

function updateParticipantSpeakingState(username, isSpeaking) {
  const participantItem = document.getElementById(`participant-${username}`);
  if (participantItem) {
    if (isSpeaking) {
      participantItem.style.border = "2px solid rgba(255, 255, 255, 0.8)";
      const statusSpan = participantItem.querySelector(".status");
      if (statusSpan) {
        statusSpan.textContent = "Speaking";
        statusSpan.style.color = "rgba(255, 255, 255, 0.9)";
      }
    } else {
      participantItem.style.border = "2px solid transparent";
      const statusSpan = participantItem.querySelector(".status");
      if (statusSpan) {
        statusSpan.textContent = "Connected";
        statusSpan.style.color = "rgba(255, 255, 255, 0.7)";
      }
    }
  }
}

// Add audio level monitoring for each participant's stream
function monitorAudioLevel(stream, username) {
  if (!stream.getAudioTracks().length) return;

  const audioContext = new window.AudioContext();
  const audioSource = audioContext.createMediaStreamSource(stream);
  const analyser = audioContext.createAnalyser();
  analyser.fftSize = 512;
  analyser.smoothingTimeConstant = 0.1;
  audioSource.connect(analyser);

  const dataArray = new Uint8Array(analyser.frequencyBinCount);
  let speakingTimeout;

  function checkAudioLevel() {
    analyser.getByteFrequencyData(dataArray);
    const average = dataArray.reduce((a, b) => a + b) / dataArray.length;

    if (average > 35) {
      // Adjust this threshold as needed
      clearTimeout(speakingTimeout);
      updateParticipantSpeakingState(username, true);
      speakingTimeout = setTimeout(() => {
        updateParticipantSpeakingState(username, false);
      }, 500);
    }

    requestAnimationFrame(checkAudioLevel);
  }

  checkAudioLevel();
}

document.addEventListener(
  "DOMContentLoaded",
  () => {
    const initializeVideoCall = async () => {
      await videoCallService.setCallbacks({
        onRemoteStreamCallback: (
          remoteStream,
          remoteUsername,
          remoteUserRole
        ) => {
          if (!remoteStream) {
            console.warn("No remote stream found");
            return;
          }

          console.log("Remote stream callback", {
            stream: remoteStream,
            username: remoteUsername,
            role: remoteUserRole,
            tracks: remoteStream
              .getTracks()
              .map((t) => ({ kind: t.kind, enabled: t.enabled })),
          });

          // Always add the participant video, the function will handle the role-specific logic
          addParticipantVideo(remoteUsername, remoteUserRole, remoteStream);
        },
        onConnectionIdCallback: (connectionId) => {
          console.log("Connection ID callback", connectionId);
        },
        onUserLeftCallback: (username) => {
          removeParticipantVideo(username);
        },
      });

      try {
        // Always try to establish connection first

        await startWebcam();
        if (localVideoRef?.srcObject) {
          videoCallService.setLocalStream(localVideoRef.srcObject);
        }

        await videoCallService.startConnection();
        console.log(
          "Connection state after start:",
          videoCallService.getConnectionState()
        );

        // Only proceed with room operations if we're connected
        if (videoCallService.getConnectionState() === "Connected") {
          const currentRoom = videoCallService.getCurrentRoom();
          if (currentRoom !== roomId) {
            if (currentRoom) {
              await videoCallService.leaveRoom();
            }
            // The connection ID is already obtained in startConnection
            await videoCallService.joinRoom(roomId);
          }
        } else if (videoCallService.getConnectionState() === "Disconnected") {
          await videoCallService.startConnection();
        } else if (videoCallService.getConnectionState() === "Connecting") {
          console.log("Connecting to server...");
        }
      } catch (error) {
        console.error("Error during video call initialization:", error);
      }
    };

    try {
      initializeVideoCall();
      // Initialize the grid with local video
      updateGridLayout();
    } catch (error) {
      console.error("Error initializing video call:", error);
    }
  },
  { once: true }
); // ensure it only runs once

window.onbeforeunload = async () => {
  console.log("Page unloading, cleaning up video call...");
  if (userRole.value === "Lecturer") {
    stopWebcam();
  }
  if (videoCallService) {
    videoCallService.stopConnection();
  }
};

const startWebcam = async () => {
  if (state.isWebcamActive) return;

  try {
    // First, try with ideal constraints
    const constraints = {
      video:
        userRole.value === "Lecturer"
          ? {
              width: { ideal: 1280 },
              height: { ideal: 720 },
              facingMode: "user",
            }
          : false,
      audio: {
        echoCancellation: true,
        noiseSuppression: true,
        autoGainControl: true,
      },
    };

    console.log("Requesting media with constraints:", constraints);
    const stream = await navigator.mediaDevices.getUserMedia(constraints);

    if (localVideoRef) {
      localVideoRef.srcObject = stream;
      // Ensure tracks are enabled based on role
      stream.getVideoTracks().forEach((track) => {
        track.enabled = userRole.value === "Lecturer" && state.isVideoEnabled;
      });
      stream.getAudioTracks().forEach((track) => {
        track.enabled = state.isAudioEnabled;
      });

      updateState({
        isWebcamActive: true,
        isVideoEnabled: userRole.value === "Lecturer" ? true : false,
        isAudioEnabled: true,
      });
      console.log("Camera and microphone access granted", {
        videoTracks: stream.getVideoTracks().length,
        audioTracks: stream.getAudioTracks().length,
      });
    }
  } catch (error) {
    console.error("Error starting webcam:", error);

    // Try with more permissive constraints if first attempt fails
    try {
      console.log("Trying fallback constraints");
      const fallbackConstraints = {
        video: userRole.value === "Lecturer",
        audio: true,
      };
      const fallbackStream = await navigator.mediaDevices.getUserMedia(
        fallbackConstraints
      );

      if (localVideoRef) {
        localVideoRef.srcObject = fallbackStream;
        // Ensure tracks are enabled based on role
        fallbackStream.getVideoTracks().forEach((track) => {
          track.enabled = userRole.value === "Lecturer" && state.isVideoEnabled;
        });
        fallbackStream.getAudioTracks().forEach((track) => {
          track.enabled = state.isAudioEnabled;
        });

        updateState({
          isWebcamActive: true,
          isVideoEnabled: userRole.value === "Lecturer",
          isAudioEnabled: true,
        });
        console.log(
          "Camera and microphone access granted with fallback options"
        );
      }
    } catch (fallbackError) {
      console.error("Fallback webcam error:", fallbackError);

      // Try one last time with just audio if video fails
      try {
        console.log("Trying audio-only fallback");
        const audioOnlyStream = await navigator.mediaDevices.getUserMedia({
          video: false,
          audio: true,
        });

        if (localVideoRef) {
          localVideoRef.srcObject = audioOnlyStream;
          audioOnlyStream.getAudioTracks().forEach((track) => {
            track.enabled = state.isAudioEnabled;
          });

          updateState({
            isWebcamActive: true,
            isVideoEnabled: false,
            isAudioEnabled: true,
          });
          console.log("Audio-only access granted");
        }
      } catch (audioOnlyError) {
        console.error("Audio-only fallback failed:", audioOnlyError);
      }
    }
  }
};

const stopWebcam = () => {
  if (localVideoRef?.srcObject) {
    const stream = localVideoRef.srcObject;
    stream.getTracks().forEach((track) => track.stop());
    localVideoRef.srcObject = null;
  }
};

const leaveRoom = async () => {
  await videoCallService.leaveRoom();
  window.location.href = "/";
};

window.leaveRoom = leaveRoom;
