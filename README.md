# 🏥 Clinic Management System

## Overview
**Clinic Management System** is an ASP.NET Core MVC application designed to manage clinic operations, including **appointments, doctors, specialties, and patient information**. The system allows secretaries to efficiently schedule appointments, manage doctors’ schedules, and maintain patient records with ease.

---

## Features

### 👨‍⚕️ Doctor & Specialty Management
- Add, edit, and delete doctors and specialties.  
- Each doctor has their own schedule (workdays and hours).

### 📅 Appointments
- Schedule new appointments with free time slots per doctor.  
- Default visit length: 30 minutes.  
- Edit and cancel appointments.  
- View appointment details in a grid.

### 🔔 Notifications
- Toast notifications for success, update, or deletion actions.

## 🛠 Technologies Used
- ASP.NET Core MVC  
- Entity Framework Core  
- SQL Server  
- C#  
- Bootstrap 5  
- AspNetCoreHero.ToastNotification (for notifications)  
- jQuery

⚙️ Usage

Secretaries can create new appointments by selecting a doctor, date, and available time.

Doctors and specialties can be managed through the respective pages.

View details and manage appointments in the appointments grid.

🌟 Future Improvements

Add authentication and user roles (Admin, Secretary, Doctor).

Implement recurring appointments.

Enhance reporting and analytics for clinic operations.

👨‍💻 Author

Mohamed Ashour – Software Engineer