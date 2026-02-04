# Changi Terminal Management System (C# / OOP)

## Project Overview

This repository contains a **robust terminal management application** developed in **C#** to automate airport terminal operations. The system is designed to manage **high-volume operational data**, including airline schedules, boarding gate logistics, and complex fee calculations, using a structured **object-oriented programming (OOP)** approach.

The project emphasizes **scalability, maintainability, and real-world system modeling** within an airport operations context.

---

## Key Features

### 🧠 Advanced OOP Architecture
- Designed a hierarchical class structure representing:
  - `Airline`
  - `Flight`
  - `BoardingGate`
  - `Terminal`
- Ensures clear separation of concerns and a maintainable codebase
- Applies core OOP principles throughout the system

### ✈️ Specialized Flight Handling
- Utilized **inheritance and polymorphism** to manage distinct flight types:
  - `NORMFlight`
  - `LWTTFlight`
- Each subclass encapsulates unique operational rules and fee logic

### 🔗 Relational Data Integration
- Implemented an automated **CSV data loader**
- Parses and integrates data from:
  - `flights.csv`
  - `airlines.csv`
- Constructs a real-time operational state of the terminal at runtime

### 💰 Dynamic Fee & Discount Engine
- Developed logic to calculate total airline fees dynamically
- Accounts for:
  - Base service charges
  - Flight-type-specific discounts
- Supports accurate financial computation at scale

### 🚪 Gate Assignment Logic
- Implemented a boarding gate tracking and assignment system
- Assigns gates based on:
  - Flight status
  - Gate availability
- Ensures efficient utilization of terminal resources

---

## Technical Stack

- **Language:** C# / .NET Core  
- **Data Handling:** CSV-based data persistence  
- **Core Concepts:**  
  - Inheritance  
  - Polymorphism  
  - Encapsulation  
  - File I/O  

---

## Academic Achievement

This project was developed to meet **A-Grade criteria** for the **PRG2 module**, demonstrating both technical precision and strong software engineering practices.

- **Data Integrity:**  
  Implemented robust CSV parsing with comprehensive error handling

- **Design Excellence:**  
  Adheres to professional naming conventions and clean architectural design

- **Problem Solving:**  
  Accurately translated real-world airport operational scenarios into a functional software solution

---

## How to Run

1. Clone the repository  
2. Open the `flight_proj.sln` solution in **Visual Studio**  
3. Ensure `flights.csv` and `airlines.csv` are located in the project root directory  
4. Build and run the project to launch the terminal management console
