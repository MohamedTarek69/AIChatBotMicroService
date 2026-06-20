<h1 align="center">🤖 AI ChatBot MicroService</h1>

<p align="center">
  <b>ASP.NET Core Web API | Groq AI | Redis | Microservices Architecture</b>
</p>

<p align="center">
  An AI-powered healthcare chatbot microservice designed to analyze patient symptoms, provide preliminary medical guidance, and maintain conversation history within the MedGuide Healthcare System.
</p>

---

## 🏗️ Project Overview

The AI ChatBot MicroService is a standalone microservice within the MedGuide Healthcare Platform.

It leverages Large Language Models (LLMs) through the Groq API to analyze user symptoms and generate intelligent healthcare responses while maintaining chat history using Redis.

---

## 🎯 Goals

- Provide AI-powered healthcare assistance.
- Analyze patient symptoms and medical information.
- Generate context-aware responses using LLMs.
- Store and retrieve conversation history.
- Integrate seamlessly with other MedGuide microservices.

---

## ✨ Main Features

| Feature | Description |
|----------|-------------|
| 🤖 AI Symptom Analysis | Analyze patient symptoms using LLMs |
| 💬 Medical Chatbot | Interactive healthcare conversations |
| 🧠 Groq Integration | AI-powered response generation |
| 📜 Chat History | Store and retrieve previous conversations |
| ⚡ Redis Caching | Fast access to conversation data |
| 🔗 Microservice Integration | Connects with Patient and Doctor services |

---

## 🧱 Architecture

The service follows a microservices-based architecture:

```text
Client
 │
 ▼
AI ChatBot MicroService
 │
 ├── Groq API
 │
 ├── Redis
 │
 ├── Patient Service
 │
 └── Doctor Service
```

### Benefits

- Independent Deployment
- Scalability
- Loose Coupling
- Maintainability
- Fault Isolation

---

## 🧰 Tech Stack

| Category | Technology |
|-----------|-------------|
| Backend | ASP.NET Core Web API |
| AI Provider | Groq API |
| Cache & History | Redis |
| Architecture | Microservices |
| Documentation | Swagger |
| Authentication | JWT Bearer |
| Version Control | Git & GitHub |

---

## 🔌 API Endpoints

### Analyze Patient Symptoms

```http
POST /api/chatbot/analyze
```

Analyzes patient symptoms and generates an AI-powered medical response.

---

### Get Chat History

```http
GET /api/chatbot/history
```

Retrieves previous chatbot conversations stored in Redis.

---

## 🧠 AI Workflow

```text
User Message
      │
      ▼
Validate Request
      │
      ▼
Collect Patient Information
      │
      ▼
Build AI Prompt
      │
      ▼
Groq LLM Processing
      │
      ▼
Generate Medical Guidance
      │
      ▼
Store Conversation In Redis
      │
      ▼
Return Response
```

---

## 📦 Core Components

### Chatbot Service

Responsible for:

- Prompt Generation
- AI Request Handling
- Response Processing
- Conversation Management

### Redis Integration

Responsible for:

- Chat History Storage
- Fast Retrieval
- Session Management

### Groq Provider

Responsible for:

- LLM Communication
- AI Response Generation
- Prompt Execution

---

## 🗄️ Data Flow

```text
Patient Input
      │
      ▼
Chatbot Controller
      │
      ▼
Chatbot Service
      │
 ┌────┴────┐
 ▼         ▼
Groq      Redis
 │          │
 ▼          ▼
AI Reply   History
```

---

## 🔒 Security

- JWT Authentication
- User-Based Chat History
- Secure API Communication
- Environment Variable Configuration

---

## 🚀 Getting Started

### Clone Repository

```bash
git clone <repository-url>
```

### Configure Environment Variables

```json
{
  "Groq": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

### Run Redis

```bash
docker run -d -p 6379:6379 redis
```

### Start Application

```bash
dotnet run
```

---

## 📌 Future Enhancements

- Multi-Conversation Support
- Medical Knowledge Base Integration
- Voice Assistant Support
- Docker Containerization
- Kubernetes Deployment

---

## 👨‍💻 Author

**Mohamed Tarek**

- GitHub: https://github.com/MohamedTarek69

---

<p align="center">
⭐ AI-powered healthcare assistance through Microservices & LLMs.
</p>
