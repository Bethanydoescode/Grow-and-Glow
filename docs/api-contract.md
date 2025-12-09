# Grow & Glow — API Contract

REST API Specification | JSON over HTTP | Version 1.0

The Grow & Glow backend exposes secure API endpoints to support authentication, mood tracking, daily insights, and profile management.

**Base URL (local development)**

```
http://localhost:5001/api
```

**Authentication**

- Bearer token required for protected routes
- Header: `Authorization: Bearer {token}`
- Content-Type: `application/json`

**Standard Response Format**

```json
{
  "success": true,
  "data": {
    /* endpoint-specific data */
  }
}
```

**Standard Error Format**

```json
{
  "success": false,
  "message": "Human-readable error description",
  "errors": ["Optional validation errors"]
}
```

**Common HTTP Status Codes**

- `200 OK` — Successful GET/PUT
- `201 Created` — Successful POST creating resource
- `400 Bad Request` — Validation error
- `401 Unauthorized` — Missing/invalid token
- `404 Not Found` — Resource not found
- `500 Internal Server Error` — Server error

---

## 1️⃣ Authentication

### **POST /auth/register**

Create a new user account.

**Request Body**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "displayName": "Alex",
  "zodiacSign": "Leo"
}
```

**Validation Rules**

- `email`: Valid email format, must be unique
- `password`: Minimum 8 characters
- `displayName`: 1-50 characters
- `zodiacSign`: One of: Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, Pisces

**Success Response** — `201 Created`

```json
{
  "success": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "displayName": "Alex",
    "zodiacSign": "Leo",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Error Response** — `400 Bad Request`

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": ["Email already exists", "Password must be at least 8 characters"]
}
```

---

### **POST /auth/login**

Authenticate a user and generate tokens.

**Request Body**

```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "displayName": "Alex",
    "zodiacSign": "Leo",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Error Response** — `401 Unauthorized`

```json
{
  "success": false,
  "message": "Invalid email or password"
}
```

---

### **POST /auth/refresh**

Generate a new access token using a refresh token.

**Request Body**

```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Error Response** — `401 Unauthorized`

```json
{
  "success": false,
  "message": "Invalid or expired refresh token"
}
```

---

## 2️⃣ User Profile

### **GET /user/me**

Retrieve authenticated user's profile.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "displayName": "Alex",
    "zodiacSign": "Leo",
    "createdAt": "2025-01-15T10:30:00Z"
  }
}
```

**Error Response** — `401 Unauthorized`

```json
{
  "success": false,
  "message": "Authentication required"
}
```

---

### **PUT /user/me**

Update user profile (displayName and/or zodiacSign).

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Request Body**

```json
{
  "displayName": "Alexander",
  "zodiacSign": "Virgo"
}
```

**Notes**

- Both fields are optional, but at least one must be provided
- `zodiacSign` must be valid zodiac sign if provided

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": {
    "displayName": "Alexander",
    "zodiacSign": "Virgo"
  }
}
```

**Error Response** — `400 Bad Request`

```json
{
  "success": false,
  "message": "Invalid zodiac sign",
  "errors": ["zodiacSign must be one of: Aries, Taurus, Gemini..."]
}
```

---

## 3️⃣ Mood Tracking

### **POST /moods**

Submit a mood entry for the authenticated user.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Request Body**

```json
{
  "moodEmoji": "😊",
  "note": "Had a great day at work!",
  "tags": ["work", "productive", "happy"]
}
```

**Field Details**

- `moodEmoji`: Single emoji character (😊, 😢, 😡, 😴, 😰, 🤗, 😐, 🥳, 😔, 😤)
- `note`: Optional, max 500 characters
- `tags`: Optional array of strings, max 5 tags

**Success Response** — `201 Created`

```json
{
  "success": true,
  "data": {
    "entryId": 12,
    "moodEmoji": "😊",
    "note": "Had a great day at work!",
    "tags": ["work", "productive", "happy"],
    "entryDate": "2025-12-08"
  }
}
```

**Error Response** — `400 Bad Request`

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": ["You already have a mood entry for today"]
}
```

**Notes**

- Only one entry per day is allowed
- `entryDate` is automatically set to current date in YYYY-MM-DD format

---

### **GET /moods/weekly-summary**

Returns the most recent 7 days of mood entries.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": [
    { "date": "2025-12-08", "moodEmoji": "😊" },
    { "date": "2025-12-07", "moodEmoji": "😐" },
    { "date": "2025-12-06", "moodEmoji": "🥳" },
    { "date": "2025-12-05", "moodEmoji": "😊" },
    { "date": "2025-12-04", "moodEmoji": "😢" }
  ]
}
```

**Notes**

- Returns up to 7 most recent entries
- Dates in YYYY-MM-DD format
- Ordered from most recent to oldest

---

### **GET /moods/streak**

Returns current consecutive logging streak count.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": {
    "streakCount": 5
  }
}
```

**Notes**

- Streak resets to 0 if a day is missed
- Today counts if entry exists

---

## 4️⃣ Daily Insights

### **GET /insights/horoscope**

Returns today's horoscope based on user's zodiac sign.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": "Today brings opportunities for growth. Trust your instincts and embrace new challenges with confidence."
}
```

**Error Response** — `503 Service Unavailable`

```json
{
  "success": false,
  "message": "Horoscope service temporarily unavailable"
}
```

**Notes**

- Zodiac sign automatically retrieved from user profile
- Data fetched from external horoscope API

---

### **GET /insights/motivation**

Returns a motivational message.

**Headers Required**

- `Authorization: Bearer {accessToken}`

**Success Response** — `200 OK`

```json
{
  "success": true,
  "data": "Every day is a new beginning. Take a deep breath and start again."
}
```

**Notes**

- May return mood-aware messages based on recent entries
- Fallback to generic motivational quotes if no mood data available

---

## Date/Time Formats

All dates and timestamps use ISO 8601 format:

- **Date only**: `YYYY-MM-DD` (e.g., "2025-12-08")
- **Date with time**: `YYYY-MM-DDTHH:mm:ssZ` (e.g., "2025-12-08T14:30:00Z")

---

## Security Requirements

- **Access Tokens**: Short expiration (15-60 minutes recommended)
- **Refresh Tokens**: Long expiration with server-side validation
- **Password Storage**: Hashed using bcrypt or similar
- **HTTPS**: Required in production environment
- **Sensitive Data**: Never return password hashes or internal IDs in responses
- **Rate Limiting**: Recommended for auth endpoints to prevent brute force attacks

---

## Change Management

All changes to this contract must be:

- Versioned using semantic versioning
- Documented in `/docs/changelog.md`
- Communicated to frontend team before deployment

**Current Version**: 1.0.0  
**Last Updated**: December 2025
