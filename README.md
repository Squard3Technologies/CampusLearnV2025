# 📘 Campus Learn Project Overview

## 🗓️ Delivery Target: 1 June

---

## ✅ Screens / Wireframes

### 🔐 Authentication & Access
1. Landing Page  
2. Login Page  
3. Register Page  
4. Forgot Password Page  
5. Terms and Conditions Page  

### 📚 Modules & Topics
6. My Modules Page  
7. Module Topics Page  
8. Topic Page  
9. New Topic Page  

### 🗂️ Topic Internals
10. About Topic Section  
11. Discussions Section  
12. Learning Material Section  
13. New Learning Material Popup  
14. Download Material *(optional interaction)*  

### ❓ Enquiries
15. My Enquiries Page  
16. New Enquiry Popup  
17. View Enquiry Popup  
18. Enquiry Management Page *(Admin)*  
19. Resolve Enquiry Popup  
20. Enquiry History Page  

### 🧠 Quizzes
21. My Quizzes Page  
22. Quiz Attempt Page  
23. Quiz History Page  
24. Quiz Attempt History Page  
25. Quiz Management Page *(Admin)*  
26. Question Popup *(Add/Edit)*  

### 💬 Chat
27. Chat Page *(Inbox view)*  
28. Chat Thread *(with specific user)*  
29. User Search Popup *(for chat initiation)*  

### 👤 User Profile & Subscriptions
30. My Profile Page  
31. Edit My Details Section  
32. Change Password Section  
33. Tutor Subscriptions Page  
34. Tutor Subscription Popup  
35. Topic Subscriptions Page  

### 🛠️ Admin Dashboard
36. Admin Dashboard Home  
37. Pending Registrations Page  
38. Process Registration Popup  
39. User Management Page  
40. User Edit / Maintain Page  
41. Module Management Page  
42. Module Edit / Maintain Page  

---

## 🔌 API Endpoints

### 🔐 Authentication
- `POST /login`
- `POST /register`
- `POST /forgot-password`

### 📚 Modules & Topics
- `GET /modules`
- `GET /topics?moduleId={id}`
- `POST /topics`
- `GET /topic-description?topicId={id}`

### 💬 Discussions & Learning Material
- `GET /discussions?topicId={id}`
- `GET /learning-material?topicId={id}`
- `POST /learning-material`
- `GET /learning-material/download?fileId={id}` *(optional)*

### 🧠 Quizzes
- `GET /quizzes?topicId={id}`
- `GET /quiz-details?quizId={id}`
- `POST /questions`
- `PUT /questions/{id}`
- `GET /active-quizzes`
- `POST /quiz-attempt`
- `GET /quiz-history`
- `GET /quiz-attempt-history?quizId={id}`

### ❓ Enquiries
- `GET /enquiries`
- `POST /enquiries`
- `GET /enquiries/{id}`
- `GET /enquiries/active`
- `POST /enquiries/{id}/resolve`
- `GET /enquiries/resolved`

### 💬 Chat
- `GET /chats`
- `GET /chats/{userId}`
- `GET /users?search=query`

### 👤 User Profile
- `GET /user`
- `PUT /user`
- `POST /change-password`

### 📩 Subscriptions
- `GET /subscriptions/tutors`
- `DELETE /subscriptions/tutors/{id}`
- `GET /tutors/available`
- `POST /subscriptions/tutors`
- `GET /subscriptions/topics`
- `POST /subscriptions/topics/unsubscribe`

### 🛠️ Admin Dashboard

#### Registrations
- `GET /admin/registrations/pending`
- `POST /admin/registrations/{id}/accept`
- `POST /admin/registrations/{id}/reject`

#### User Management
- `GET /admin/users`
- `POST /admin/users/{id}/deactivate`
- `POST /admin/users/{id}/activate`
- `PUT /admin/users/{id}`

#### Module Management
- `GET /admin/modules`
- `POST /admin/modules`
- `PUT /admin/modules/{id}`
- `POST /admin/modules/{id}/deactivate`
- `POST /admin/modules/{id}/activate`


# 🐳 Docker Configuration
## Creating internal network
    docker network create campuslearn-net

## Setting up MSSQL Server
    docker pull mcr.microsoft.com/mssql/server
    docker run --restart always -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=9w#ZCunZM3" --name campuslearn-mssql -p 1404:1433 --network campuslearn-net -d mcr.microsoft.com/mssql/server

## Setting Web API
    docker pull campuslearn-api:1.3
    docker run --restart always --name campuslearn-api -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://0.0.0.0:8080 --network campuslearn-net -d campuslearn-api:1.3
    

