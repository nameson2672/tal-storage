# Tal Storage Service

The **Tal Storage Service** is an ASP.NET Core 9-based file upload service that supports **multipart uploads** to **Amazon S3** and stores **file metadata** in a **PostgreSQL** database. This service is part of the **Tal** app and is designed for efficient file handling and secure storage.
![Tal Storage Design](https://raw.githubusercontent.com/nameson2672/tal-storage/refs/heads/main/static/image.png)
## Features

- Multipart file upload handling.
- File metadata storage in **PostgreSQL**.
- Integration with **Amazon S3** for file storage.
- Secure upload process with validation and error handling.

## Prerequisites

Make sure you have the following prerequisites installed:

- **.NET 9 SDK**
- **PostgreSQL** instance
- **Amazon S3** account and credentials

## Installation

### 1. Clone the repository

```bash
git clone https://github.com/nameson2672/tal-storage.git
cd tal-storage
