# Corporate Banking Payment Application

## Introduction

### **Purpose**
This document outlines the functional and non-functional requirements for the Corporate Banking Payment Application. It includes system architecture, user roles, and their permissions. This application is built with Angular for the frontend and .NET for the backend.

### **Scope**
The Corporate Banking Payment Application facilitates banking operations for corporate clients, banks, and super administrators. Key features include customer onboarding, client management, payment processing, salary disbursement, and report generation.

### **Definitions**
- **CRUD:** Create, Read, Update, Delete
- **OAuth:** Open Authorization
- **CAPTCHA:** Completely Automated Public Turing test to tell Computers and Humans Apart
- **JWT:** JSON Web Token
- **API:** Application Programming Interface

## Overall Description

### **Product Perspective**
This web-based application integrates with banking systems via secure APIs. It uses OAuth 2.0 for authentication and authorization.

### **Product Functions**
- User authentication and authorization
- Bank and client management
- Customer onboarding and verification
- Payment processing and approval
- Salary disbursement
- Document upload and management
- Batch transaction processing
- Report generation

### **User Classes**
- **Super Admin:** Manages banks, clients, and generates reports.
- **Bank User:** Approves/rejects payments, generates reports, manages documents.
- **Client User:** Manages beneficiaries and employees, uploads documents, processes payments, disburses salaries, and views reports.

### **Operating Environment**
- **Backend:** .NET Core
- **Frontend:** Angular
- **Database:** SQL Server
- **Authentication:** OAuth 2.0 (JWT)
- **Browser Support:** Chrome, Firefox, Edge, Safari

## System Features

### **Super Admin Features**
- Manage banks and clients (CRUD operations).
- Onboard clients with document uploads.
- Generate system usage reports and audit logs.

### **Bank User Features**
- Approve/reject client payment requests.
- Generate transaction reports.
- Upload and manage documents.

### **Client User Features**
- Manage beneficiaries and employees (CRUD operations).
- Process payments and disburse salaries.
- Generate transaction and salary reports.

## External Interface Requirements

### **User Interface**
The application offers intuitive, responsive interfaces for each user role.

### **APIs**
RESTful APIs are used for customer onboarding, payment processing, and report generation.

### **Authentication**
OAuth 2.0 ensures secure user authentication and authorization.

## Functional Requirements
1. Super Admin manages bank records (CRUD).
2. Super Admin onboards clients, including document uploads.
3. Super Admin manages and verifies clients.
4. Bank users approve/reject payment requests.
5. Bank users generate transaction reports.
6. Client users manage beneficiaries and employees (CRUD).
7. Client users process payments and disburse salaries.
8. Client users generate transaction and salary reports.
9. Support for document uploads and management.
10. Support for batch transaction processing.
11. CAPTCHA for form submissions.

## Non-Functional Requirements

- **Security:** Ensure data privacy with encryption.
- **Availability:** 99.9% uptime.
- **Scalability:** Handle increasing loads efficiently.
- **Performance:** Ensure response times within 2 seconds.
- **Maintainability:** Easy updates and bug fixes.

## Security Requirements

- OAuth 2.0 for secure authentication and authorization.
- CAPTCHA for preventing automated form submissions.
- Data encryption at rest and in transit.
- Log all activities for auditing.

## Development Environment

- **Backend:** .NET Core
- **Frontend:** Angular
- **Database:** SQL Server
- **Version Control:** Git

## System Flow

### **User Authentication**
1. User enters login credentials.
2. System validates credentials via OAuth 2.0 and generates a JWT token.
3. User is redirected to their dashboard based on their role.

### **Customer Onboarding**
1. Super Admin initiates the client onboarding process.
2. Documents are uploaded and verified.
3. System notifies the verification team.
4. The client is approved or rejected, and the client is notified.

### **Payment Processing**
1. Client selects a beneficiary and submits payment details.
2. Payment is sent for approval by the Bank User.
3. Upon approval, the payment is processed.

### **Salary Disbursement**
1. Client manages employee records.
2. Salaries can be disbursed individually or in batch.
3. Notifications and receipts are generated after disbursement.

### **Document Management**
1. Users can upload documents related to clients, employees, or transactions.
2. Documents are securely stored and accessed based on user permissions.

### **Report Generation**
1. Users can generate reports (e.g., transaction reports).
2. Reports can be filtered and exported in formats like PDF or Excel.
