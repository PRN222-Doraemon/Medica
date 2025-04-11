# MindSpace - School Mental Health Management System
<div align="center">
    <img src="./profile/readme/images/logo.jpg" alt="MindSpace" height='80px'/>
    <p>MindSpace is a web application that provides mental health services for high school students and parents.</p>
</div>
<div align='center'>
<img src="https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff"> 
<img src="https://custom-icon-badges.demolab.com/badge/Microsoft%20SQL%20Server-CC2927?logo=mssqlserver-white&logoColor=white"> 
<img src="https://img.shields.io/badge/Redis-%23DD0031.svg?logo=redis&logoColor=white"> 
<img src="https://img.shields.io/badge/Docker-Yes-green"> 
<img src="https://img.shields.io/badge/Server-Yes-green"> 
<img src="https://img.shields.io/badge/API-Yes-green">
<br>
</div>

## Table of Contents
<ol start="0"> 
    <li><a href="#intro">Introduction</a></li>
    <li><a href="#tech">Tech Stacks</a></li>
    <li><a href="#uc-diagram">Use Case Diagram</a></li>
    <li><a href="#uc-diagram">Architecture Diagram</a></li>
    <li><a href="#db-design">Database Design</a></li>
    <li><a href="#screen-flow"> Screen Flow</a></li>
    <li><a href="#team-members">Team Members</a></li>
    <li>
        <a href="#app-a">Appendix A</a>
    </li>
</ol>


<a id="intro"></a>
## 0. Introduction
The increasing concern for students' mental health in Vietnamese schools highlights the need for a structured psychological support system. 
Currently, many schools lack a standardized approach to managing students' well-being. 
MindSpace aims to bridge this gap by providing a digital platform where students can access professional counseling services, schools can monitor overall student mental health trends, and psychologists can offer their expertise.

## Core Features

- **Psychological Assessments**: Students can take structured psychological tests to evaluate their mental health.
- **Supporting Programs**: Schools provide students with three free Supporting Programs through workshops and webinars for specific targets (e.g., school mental health, anxiety, etc.), featuring professional psychologists.
- **Appointment Scheduling**: Students can book sessions with licensed psychologists directly through the platform.
- **Confidential Chat System**: Secure messaging between students and an AI chatbot for follow-up support.
- **Mental Health Resources**: A curated library of self-help materials, articles, and exercises for students.
- **Administrative Dashboard for Schools**: Schools can monitor student participation and access general mental health trends (without violating privacy).
- **Psychologist Management System**: Tools for psychologists to manage their schedules, session notes, and earnings.

## Target Users

- **Primary Users**:
  - School managers
  - Students
  - Psychologists
- **Secondary Users**:
  - Parents
    
<a id="tech"></a>
## 1. Tech Stacks

## Client & Server Technologies  

| Client | Description | Server | Description |
|--------|------------|--------|------------|
| ![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?logo=typescript&logoColor=white) | Strongly typed JavaScript | ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white) | Backend framework for building APIs |
| ![Next.js](https://img.shields.io/badge/Next.js-000000?logo=nextdotjs&logoColor=white) | React framework with server-side rendering | ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?logo=dotnet&logoColor=white) | ORM for database interactions |
| ![React Native](https://img.shields.io/badge/React%20Native-61DAFB?logo=react&logoColor=white) | Mobile app development | ![ASP.NET Identity](https://img.shields.io/badge/ASP.NET%20Identity-512BD4?logo=dotnet&logoColor=white) | Authentication and authorization |
| ![Hero UI](https://img.shields.io/badge/Hero%20UI-38B2AC?logo=heroicons&logoColor=white) | UI component library | ![Serilog](https://img.shields.io/badge/Serilog-4B8BBE?logo=serilog&logoColor=white) | Structured logging framework |
| ![Tailwind CSS](https://img.shields.io/badge/Tailwind%20CSS-06B6D4?logo=tailwindcss&logoColor=white) | Utility-first CSS framework | ![Quartz.NET](https://img.shields.io/badge/Quartz.NET-004080?logo=clockify&logoColor=white) | Job scheduling library |
| ![Axios](https://img.shields.io/badge/Axios-5A29E4?logo=axios&logoColor=white) | HTTP client for API requests | ![JWT](https://img.shields.io/badge/JWT%20Auth-000000?logo=jsonwebtokens&logoColor=white) | Token-based authentication |
| | | ![Postman](https://img.shields.io/badge/Postman-FF6C37?logo=postman&logoColor=white) | API testing tool |
| | | ![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=white) | API documentation generator |
| | | ![SignalR](https://img.shields.io/badge/SignalR-0088CC?logo=microsoft&logoColor=white) | Real-time communication |
| | | ![Mediator](https://img.shields.io/badge/Mediator-0078D4?logo=microsoft&logoColor=white) | Decouples communication between components |
| | | ![CQRS](https://img.shields.io/badge/CQRS-0078D4?logo=microsoft&logoColor=white) | Separates read/write operations |
| | | ![Excel API](https://img.shields.io/badge/Excel%20API-217346?logo=microsoft-excel&logoColor=white) | Microsoft Excel data processing |
| | | ![Gmail API](https://img.shields.io/badge/Gmail%20API-D14836?logo=gmail&logoColor=white) | Email automation and integration |

---

## Database & API Deployment  

The **Database** stack includes data storage solutions, while **API Deployment** contains tools for containerization, automation, and cloud services.  

| Database | Description | API Deployment | Description |
|----------|------------|---------------|------------|
| ![SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white) | Cloud-based relational database | ![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white) | Containerization platform |
| ![Redis](https://img.shields.io/badge/Redis-DD0031?logo=redis&logoColor=white) | In-memory caching database | ![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?logo=githubactions&logoColor=white) | CI/CD automation |
| | | ![Azure](https://img.shields.io/badge/Azure%20Cloud-0078D4?logo=microsoftazure&logoColor=white) | Backend deployment |
| | | ![ngrok](https://img.shields.io/badge/ngrok-1F1F1F?logo=ngrok&logoColor=white) | Local development tunneling |

---

## Others  

Additional services and third-party APIs that enhance functionality, such as AI, payments, video streaming, and file storage.  

| Service | Description |
|---------|------------|
| ![Gemini API](https://img.shields.io/badge/Gemini%20API-4285F4?logo=google&logoColor=white) | AI-powered chatbot service |
| ![Stripe](https://img.shields.io/badge/Stripe-008CDD?logo=stripe&logoColor=white) | Online payment processing |
| ![WebRTC](https://img.shields.io/badge/WebRTC-333333?logo=webrtc&logoColor=white) | Peer-to-peer video streaming |
| ![Cloudinary](https://img.shields.io/badge/Cloudinary-3448C5?logo=cloudinary&logoColor=white) | Image and video storage |


<a id="uc-diagram"></a>
## 2. Use Case Diagram
<img src="./profile/readme/images/MindSpace-UsecaseDiagram.png" alt="MindSpace-UsecaseDiagram" />

<a id="db-design"></a>
## 3. Database Design
<img src="./profile/readme/images/MindSpace-ERD.png" alt="MindSpace-ERD" />

<a id="architecture"></a>
## 4. Architecture Diagram
<img src="./profile/readme/images/MindSpace-ArchitectureDiagram.png" alt="MindSpace-ArchitectureDiagram" />

<a id="screen-flow"></a>
## 5. Screen Flow

### 5.1. Student Screen Flow:
<img src="./profile/readme/images/MindSpace-StudentScreenflow.png" alt="MindSpace-StudentScreenflow" />

### 5.2. Parent Screen Flow:
<img src="./profile/readme/images/MindSpace-ParentScreenflow.png" alt="MindSpace-ParentScreenflow" />

### 5.3. Psychologist Screen Flow:
<img src="./profile/readme/images/MindSpace-PsychologistScreenFlow.png" alt="MindSpace-PsychologistScreenflow" />

### 5.4. School Manager Screen Flow:
<img src="./profile/readme/images/MindSpace-SchoolManagerScreenflow.png" alt="MindSpace-SchoolManagerScreenflow" />

### 5.5. Admin Screen Flow:
<img src="./profile/readme/images/MindSpace-AdminScreenflow.png" alt="MindSpace-AdminScreenflow" />

<a id="team-members"></a>
## 6. Team members
- [Vu Kim Duy](https://github.com/AnonyFriday): Project Leader, Web Front-End, Back-End Developer
- [Phan Tuan Dat](https://github.com/imbatcat): Web Front-End, Back-End Developer
- [Vo Thi Mai Hoa](https://github.com/vohoa2004): Web Front-End, Back-End Developer
- [Nguyen Thi Bich Duyen](https://github.com/cuckoo01): Web Front-End Developer
- [Tran Van Tien Dat](https://github.com/datTOK): Web Front-End Developer
- [Le Minh Quan](https://github.com/QuanLM270302): Mobile Front-End Developer


<a id="app-a"></a>

# Appendix A
