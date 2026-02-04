//==========================================================
// Student Number	: S10260014E
// Student Name	: Heng Jue Wei Tevin
// Partner Name	: Queh Ching Kai Javier
//==========================================================


// Remember to update the modified details to both Airline and Flights class.
// 
using PRG_2_ASSG;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

Dictionary<string, Airline> airlines = new Dictionary<string, Airline>(); //Airline code, Airline
Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
Dictionary<string, Flight> flights = new Dictionary<string, Flight>(); //FLight Number, Flight

LoadAirlines(airlines);
LoadBoardingGates(boardingGates);
LoadFlights(flights);
Terminal terminal5 = new Terminal("Changi Airport Terminal 5", airlines, flights, boardingGates);
foreach (BoardingGate gates in boardingGates.Values)
{
    terminal5.GateFees.Add(gates.GateName, 300.0); //Adding each of the boarding gate fees to the dictionary in Terminal class GateFees
}
static async Task<T> ProcessDataAsync<T>(HttpClient client, string url)
{
    await using Stream stream =
        await client.GetStreamAsync(url);
    var obj =
        await JsonSerializer.DeserializeAsync<T>(stream);
    return obj;
}

Console.WriteLine();
while (true)
{
    Console.WriteLine(); //Spacing
    DisplayMenu();
    int choice = 0;
    while (true)
    {
        try
        {
            Console.WriteLine("Please select your option: ");
            choice = Convert.ToInt32(Console.ReadLine());
            if (choice < 0 || choice > 10)
            {
                throw new Exception();
            }
            break;
        }

        catch (FormatException)
        {
            Console.WriteLine("Please pick one of the options.");
            continue;
        }

        catch (Exception)
        {
            Console.WriteLine("The input is not part of the options shown above.");
            Console.WriteLine("Please try again.");
            continue;

        }
    }
    if (choice == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else
    {
        if (choice == 1)
        {
            ListAllFlights(flights, airlines);
        }

        else if (choice == 2)
        {
            ListBoardingGates(boardingGates);
        }

        else if (choice == 3)
        {
            AssignBoardingGateToFlight(boardingGates, flights);
        }

        else if (choice == 4)
        {
            CreateFlight(flights, airlines);
        }

        else if (choice == 5)
        {
            DisplayDetailsFromAirline(airlines, boardingGates);
        }

        else if (choice == 6)
        {
            ModifyFlightDetails(airlines, boardingGates, flights);
        }

        else if (choice == 7)
        {
            DisplayFlightSchedule(flights, airlines, boardingGates);
        }
        else if (choice == 8)
        {
            ProcessUnassignedFlights(flights, boardingGates, airlines);
        }
        else if (choice == 9)
        {
            DisplayAirlineFee(flights, boardingGates);
        }

        else if (choice == 10)
        {

            using HttpClient client = new();
            string twoHourForecast = "https://api.data.gov.sg/v1/environment/2-hour-weather-forecast";
            var forecast = await ProcessDataAsync<Rootobject>(client, twoHourForecast);
            var areas = forecast.area_metadata;
            string target = areas[9].name; //Changi 
            var twoHourForecastItem = forecast.items[0];
            foreach (var area in twoHourForecastItem.forecasts)
            {
                if (area.area == target)
                {
                    Console.WriteLine("============================================= \nWeather Forecast for Changi Airport \n=============================================");
                    Console.WriteLine("\nThe current weather in Changi is " + area.forecast + ".");
                    if (area.forecast == "Thundery Showers" || area.forecast == "Heavy Thundery Showers" || area.forecast == "Heavy Thundery Showers with Gusty Winds")
                    {
                        Console.WriteLine("Please expect delayed flights");
                        Console.WriteLine("Checking for delayed flights...");
                        int count = 0;
                        foreach (Flight flight in flights.Values)
                        {
                            string originalStatus = flight.Status;
                            if (flight.Origin == "Singapore (SIN)") //Check if the flight is departing from Singapoere
                            {
                                flight.Status = "Delayed";
                                foreach (Airline airline in airlines.Values)
                                {
                                    if (airline.Flights.ContainsKey(flight.FlightNumber))
                                    {
                                        airline.Flights[flight.FlightNumber].Status = "Delayed";
                                    }
                                }
                                Console.WriteLine("\nFlight " + flight.FlightNumber);
                                Console.WriteLine("Origin: " + flight.Origin);
                                Console.WriteLine("Destination: " + flight.Destination);
                                Console.WriteLine("Expected Time: " + flight.ExpectedTime);
                                Console.WriteLine("Boarding Gate: " + GetBoardingGateforFlight(flight, boardingGates));
                                Console.WriteLine("Status: " + originalStatus + " -->  DELAYED");
                                count++;
                            }
                        }
                        if (count == 0)
                        {
                            Console.WriteLine("\nNo flights are delayed.");
                        }
                        else
                        {
                            Console.WriteLine("\nAll delayed flights have been updated.");
                            Console.WriteLine("A total of " + count + " flights have been delayed.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nThe weather is safe for flights to depart.");
                        Console.WriteLine("Have a safe flight :)");
                    }
                }
            }
        }
    }
}

void LoadAirlines(Dictionary<string, Airline> airlines)
{
    Console.WriteLine("Loading Airlines...");
    string[] airlineFile = File.ReadAllLines("airlines.csv");
    int airlineCount = 0;
    for (int i = 1; i < airlineFile.Length; i++)
    {
        string[] temp = airlineFile[i].Split(',');
        Airline airline = new Airline(temp[0], temp[1]);
        airlineCount++;
        airlines.Add(airline.Code, airline);

    }
    Console.WriteLine($"{airlineCount} Airlines Loaded!");

}


void LoadBoardingGates(Dictionary<string, BoardingGate> boardingGates)
{
    Console.WriteLine("Loading Boarding Gates...");
    string[] boardinggateFile = File.ReadAllLines("boardinggates.csv");
    int boardinggateCount = 0;
    for (int i = 1; i < boardinggateFile.Length; i++)
    {
        string[] temp = boardinggateFile[i].Split(',');
        BoardingGate boardingGate = new BoardingGate(temp[0], bool.Parse(temp[1]), bool.Parse(temp[2]), bool.Parse(temp[3]), null);
        boardinggateCount++;
        boardingGates.Add(boardingGate.GateName, boardingGate);
    }
    Console.WriteLine($"{boardinggateCount} Boarding Gates Loaded!");
}

void LoadFlights(Dictionary<string, Flight> flights)
{
    Console.WriteLine("Loading Flights...");
    string[] flightFile = File.ReadAllLines("flights.csv");
    int flightCount = 0;
    Flight flight;
    for (int i = 1; i < flightFile.Length; i++)
    {
        string[] temp = flightFile[i].Split(',');
        if (temp[4] == "CFFT")
        {
            flight = new CFFTFlight(temp[0], temp[1], temp[2], DateTime.Parse(temp[3]), "Scheduled");

        }

        else if (temp[4] == "DDJB")
        {
            flight = new DDJBFlight(temp[0], temp[1], temp[2], DateTime.Parse(temp[3]), "Scheduled");

        }

        else if (temp[4] == "LWTT")
        {
            flight = new LWTTFlight(temp[0], temp[1], temp[2], DateTime.Parse(temp[3]), "Scheduled");

        }

        else
        {

            flight = new NORMFlight(temp[0], temp[1], temp[2], DateTime.Parse(temp[3]), "Scheduled");

        }
        flights.Add(temp[0], flight);
        flightCount++;

    }
    Console.WriteLine($"{flightCount} Flights Loaded!");
    foreach (Airline airline in airlines.Values)
    {
        foreach (Flight f in flights.Values)
        {
            string[] temp = f.FlightNumber.Split(' ');
            if (airline.Code == temp[0])
            {
                airline.Flights.Add(f.FlightNumber, f);
            }
        }
    }
}

void DisplayMenu()
{
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process Unassigned Flights");
    Console.WriteLine("9. Display Airline Fees");
    Console.WriteLine("10. Check for Bad Weather");
    Console.WriteLine("0. Exit");
}

void ListAllFlights(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines)
{
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine($"Flight Number\tAirline Name {"Origin",15} {"Destination",20} \t\tDeparture/Arrival Time");
    foreach (Flight flight in flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber} {terminal5.GetAirlineFromFlight(flight).Name,25} {flight.Origin,17}\t{flight.Destination,15}\t{flight.ExpectedTime,28}");
    }
}

void ListBoardingGates(Dictionary<string, BoardingGate> baordingGates)
{
    Console.WriteLine("======================================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=====================================================\r\n");
    Console.WriteLine($"Gate Name {"CFFT",12} {"DDJB",15} {"LWTT",15}");
    foreach (BoardingGate boardingGate in boardingGates.Values)
    {
        Console.WriteLine($"{boardingGate.GateName}\t{boardingGate.SupportsCFFT,15}\t{boardingGate.SupportsDDJB,15}\t{boardingGate.SupportsLWTT,15}");
    }
}

string GetSpecialRequestCode(Flight flight)
{
    if (flight is DDJBFlight)
    {
        return "DDJB";
    }

    else if (flight is CFFTFlight)
    {
        return "CFFT";
    }

    else if (flight is LWTTFlight)
    {
        return "LWTT";
    }
    else
    {
        return "None";
    }
}

void AssignBoardingGateToFlight(Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Flight> flights)
{
    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
    string assgnFlightNum = "";
    string assgnGateName = "";
    while (true)
    {

        try
        {
            Console.WriteLine("Enter Flight Number: ");
            string inputFlightNum = Console.ReadLine();
            if (flights.ContainsKey(inputFlightNum))
            {
                assgnFlightNum = inputFlightNum;
                break;
            }
            else
            {
                throw new Exception();
            }
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Flight Number!");
            continue;
        }
    }

    while (true)
    {
        try
        {
            Console.WriteLine("Enter Boarding Gate Name: ");
            string inputGateName = Console.ReadLine();
            if (boardingGates.ContainsKey(inputGateName))
            {

                if (boardingGates[inputGateName].Flight != null)
                {
                    Console.WriteLine("Boarding Gate is already assigned to a flight!");
                    continue;
                }
                else
                {
                    boardingGates[boardingGates[inputGateName].GateName].Flight = flights[assgnFlightNum];
                    assgnGateName = inputGateName;
                    break;
                }
            }

            else
            {
                throw new Exception();
            }

        }
        catch (Exception)
        {
            Console.WriteLine("Invalid Boarding Gate Name!");
            continue;
        }

    }
    Console.WriteLine("Flight Number: " + flights[assgnFlightNum].FlightNumber);
    Console.WriteLine("Origin: " + flights[assgnFlightNum].Origin);
    Console.WriteLine("Destination: " + flights[assgnFlightNum].Destination);
    Console.WriteLine("Expected Time: " + flights[assgnFlightNum].ExpectedTime);
    Console.WriteLine("Special Request Code: " + GetSpecialRequestCode(flights[assgnFlightNum]));
    Console.WriteLine("Boarding Gate Name: " + boardingGates[assgnGateName].GateName);
    Console.WriteLine("Supports DDJB: " + boardingGates[assgnGateName].SupportsDDJB);
    Console.WriteLine("Supports CFFT: " + boardingGates[assgnGateName].SupportsCFFT);
    Console.WriteLine("Supports LWTT: " + boardingGates[assgnGateName].SupportsLWTT);

    string input = "";
    while (true)
    {

        try
        {
            Console.WriteLine("Would you like to update the status of the flight? (Y/N) ");
            input = Console.ReadLine();
            if (input.ToUpper() == "Y")
            {

                break;
            }

            else if (input.ToUpper() == "N")
            {
                break;
            }

            else
            {
                throw new Exception();
            }
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Input!");
            continue;
        }
    }
    int newStatus = 0;
    if (input == "Y")
    {

        while (true)
        {
            try
            {
                Console.WriteLine("1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.WriteLine("Please select the new status of the flight: ");
                newStatus = Convert.ToInt32(Console.ReadLine());
                if (newStatus < 1 || newStatus > 3)
                {
                    throw new FormatException();
                }
                break;
            }

            catch (FormatException)
            {
                Console.WriteLine("Invalid Option!");
                Console.WriteLine("Please select a valid option!");
                continue;
            }


        }
    }

    else
    {
        Console.WriteLine("Status Unchanged!");
    }

    if (newStatus == 1)
    {
        flights[assgnFlightNum].Status = "Delayed";
    }

    else if (newStatus == 2)
    {
        flights[assgnFlightNum].Status = "Boarding";
    }

    else if (newStatus == 3)
    {
        flights[assgnFlightNum].Status = "On Time";
    }
    // Changing the flights in the airline class too
    foreach (Airline airline in airlines.Values)
    {
        if (airline.Flights.ContainsKey(assgnFlightNum))
        {
            airline.Flights[assgnFlightNum].Status = flights[assgnFlightNum].Status;
        }
    }

    Console.WriteLine($"Flight {flights[assgnFlightNum].FlightNumber} has been assigned to Boarding Gate {assgnGateName}!");
}

void CreateFlight(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines)
{
    Console.WriteLine("=============================================\r\nCreate a Flight\r\n=============================================\r\n");
    string flightNum = "";
    DateTime expTime = new DateTime();
    string SpecialRequestCode = "";

    while (true)
    {
        try
        {
            Console.Write("Enter Flight Number: ");
            string inputFlightNum = Console.ReadLine();
            string[] arrayFlight = inputFlightNum.Split(' ');
            if (flights.ContainsKey(inputFlightNum))
            {
                Console.WriteLine("Flight Number already exists!");
                continue;
            }
            else if (!airlines.ContainsKey(arrayFlight[0]))
            {
                Console.WriteLine("Airline Code does not exist!");
                throw new Exception();
            }
            flightNum = inputFlightNum;
            break;
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Flight Number!");
            continue;
        }
    }
    Console.Write("Enter Origin: ");
    string origin = Console.ReadLine();
    Console.Write("Enter Destination: ");
    string destination = Console.ReadLine();

    while (true)
    {
        try
        {
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            expTime = DateTime.Parse(Console.ReadLine());
            break;
        }

        catch (FormatException)
        {
            Console.WriteLine("Invalid Date Format!");
            continue;
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Date!");
            continue;
        }
    }

    while (true)
    {
        try
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string inputSpecialRequestCode = Console.ReadLine();
            if (inputSpecialRequestCode.ToUpper() != "CFFT" && inputSpecialRequestCode.ToUpper() != "DDJB" && inputSpecialRequestCode.ToUpper() != "LWTT" && inputSpecialRequestCode.ToUpper() != "NONE")
            {
                throw new Exception();
            }
            SpecialRequestCode = inputSpecialRequestCode;
            break;
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Special Request Code!");
            continue;
        }
    }
    string[] temp = flightNum.Split(' ');
    Airline airline = airlines[temp[0]];
    if (SpecialRequestCode == "CFFT")
    {
        flights.Add(flightNum, new CFFTFlight(flightNum, origin, destination, expTime, "Scheduled"));
    }

    else if (SpecialRequestCode == "DDJB")
    {
        flights.Add(flightNum, new DDJBFlight(flightNum, origin, destination, expTime, "Scheduled"));
    }

    else if (SpecialRequestCode == "LWTT")
    {
        flights.Add(flightNum, new LWTTFlight(flightNum, origin, destination, expTime, "Scheduled"));
    }

    else
    {
        flights.Add(flightNum, new NORMFlight(flightNum, origin, destination, expTime, "Scheduled"));
    }
    if (airline.AddFlight(flights[flightNum]) == false)
    {
        Console.WriteLine("Flight Number already exists!");
        Console.WriteLine("Redirecting you back to the start.");
        CreateFlight(flights, airlines);
    }
    else
    {
        Console.WriteLine($"Flight {flightNum} has been added!");
    }
    string newline = "";
    Console.WriteLine(SpecialRequestCode);
    if (SpecialRequestCode == "None")
    {
        newline = $"{flightNum},{origin},{destination},{expTime},{""}";
    }
    else
    {
        newline = $"{flightNum},{origin},{destination},{expTime},{SpecialRequestCode}";
    }
    File.AppendAllText("flights.csv", newline);
    while (true)
    {
        try
        {
            Console.WriteLine("Would you like to add another flight? (Y/N)");
            string input = Console.ReadLine();
            if (input.ToUpper() == "Y")
            {
                CreateFlight(flights, airlines);
                break;
            }

            else if (input.ToUpper() == "N")
            {
                break;
            }

            else
            {
                throw new Exception();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid Input!");
            continue;
        }
    }
}

string GetBoardingGateforFlight(Flight flight, Dictionary<string, BoardingGate> boardingGates)
{
    foreach (BoardingGate gate in boardingGates.Values)
    {
        if (gate.Flight == flight)
        {
            return gate.GateName;
        }
    }

    return "Unassigned";
}


void DisplayDetailsFromAirline(Dictionary<string, Airline> airlines, Dictionary<string, BoardingGate> boardingGates)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (Airline airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name}");
    }
    string airlineCode = "";
    while (true)
    {
        try
        {
            Console.WriteLine("Enter Airline Code: ");
            string inputAirlineCode = Console.ReadLine();
            if (airlines.ContainsKey(inputAirlineCode.ToUpper()))
            {
                airlineCode = inputAirlineCode;
                break;
            }
            else
            {
                throw new Exception();
            }
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Airline Code!");
            continue;
        }
    }
    Console.WriteLine("=============================================\r\nList of Flights for Airline " + airlines[airlineCode].Name + "\r\n=============================================\r\n");
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");
    Airline selectedAirline = airlines[airlineCode];
    foreach (Flight flight in selectedAirline.Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {selectedAirline.Name,-22} {flight.Origin,-22} {flight.Destination,-22} {flight.ExpectedTime}");
    }

    Console.WriteLine();
    string flightNum = "";
    while (true)
    {
        try
        {
            Console.WriteLine("Enter a Flight Number to view more details: ");
            string inputFlightNum = Console.ReadLine();
            if (selectedAirline.Flights.ContainsKey(inputFlightNum))
            {
                flightNum = inputFlightNum;
                break;
            }
            else
            {
                throw new Exception();
            }
        }

        catch (FormatException)
        {
            Console.WriteLine("Invalid Flight Number");
            continue;
        }

        catch (Exception)
        {
            Console.WriteLine("Flight Number is not a part of the airline.");
            Console.WriteLine("Please try again.");
            continue;
        }
    }
    Console.WriteLine("Flight Number: " + selectedAirline.Flights[flightNum].FlightNumber);
    Console.WriteLine("Origin: " + selectedAirline.Flights[flightNum].Origin);
    Console.WriteLine("Destination: " + selectedAirline.Flights[flightNum].Destination);
    Console.WriteLine("Expected Time: " + selectedAirline.Flights[flightNum].ExpectedTime);
    Console.WriteLine("Special Request Code: " + GetSpecialRequestCode(selectedAirline.Flights[flightNum]));
    Console.WriteLine("Boarding Gate: " + GetBoardingGateforFlight(selectedAirline.Flights[flightNum], boardingGates));
}

void ModifyFlightDetails(Dictionary<string, Airline> airlines, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Flight> flights)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (Airline airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name}");
    }
    string airlineCode = "";
    while (true)
    {
        try
        {
            Console.WriteLine("Enter Airline Code: ");
            string inputAirlineCode = Console.ReadLine();
            if (inputAirlineCode == "")
            {
                throw new Exception();
            }

            else if (airlines.ContainsKey(inputAirlineCode))
            {
                airlineCode = inputAirlineCode;
                break;
            }
            else
            {
                Console.WriteLine("Airline code is not found.");
                throw new Exception();
            }
        }

        catch (Exception)
        {
            Console.WriteLine("Invalid Airline Code!");
            continue;
        }

    }
    Console.WriteLine($"List of Flights for {airlines[airlineCode].Name}");
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");
    foreach (Flight flight in airlines[airlineCode].Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {airlines[airlineCode].Name,-22} {flight.Origin,-22} {flight.Destination,-22} {flight.ExpectedTime}");
    }
    string ChosenFlightNum = "";
    while (true)
    {
        try
        {
            Console.WriteLine("Choose an existing Flight Number to modify or delete: ");
            string inputFlightNum = Console.ReadLine();
            if (airlines[airlineCode].Flights.ContainsKey(inputFlightNum))
            {
                ChosenFlightNum = inputFlightNum;
                break;
            }

            else if (inputFlightNum == "")
            {

                throw new Exception();
            }

            else
            {
                Console.WriteLine("Flight Number does not exist!");
                throw new Exception();
            }

        }
        catch (Exception)
        {
            Console.WriteLine("Invalid Flight Number!");
            continue;
        }
    }
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    int choice = 0;
    while (true)
    {
        try
        {
            Console.WriteLine("Choose an option: ");
            choice = Convert.ToInt32(Console.ReadLine());
            if (choice < 1 || choice > 2)
            {
                Console.WriteLine("Invalid Option!");
                throw new FormatException();
            }
            break;
        }

        catch (FormatException)
        {
            Console.WriteLine("Please pick option 1 or 2.");
            continue;
        }
    }
    if (choice == 1)
    {
        Console.Write("Enter New Origin: ");
        string newOrigin = Console.ReadLine();
        Console.Write("Enter New Destination: ");
        string newDestination = Console.ReadLine();
        DateTime newExpTime = new DateTime();
        while (true)
        {
            try
            {
                Console.Write("Enter New Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                newExpTime = DateTime.Parse(Console.ReadLine());
                break;
            }

            catch (FormatException)
            {
                Console.WriteLine("Invalid Date Format!");
                continue;
            }

            catch (Exception)
            {
                Console.WriteLine("Invalid Date!");
                continue;
            }
        }
        airlines[airlineCode].Flights[ChosenFlightNum].Origin = newOrigin;
        airlines[airlineCode].Flights[ChosenFlightNum].Destination = newDestination;
        airlines[airlineCode].Flights[ChosenFlightNum].ExpectedTime = newExpTime;
        Console.WriteLine("Flight Updated!");
        Console.WriteLine("Flight Number: " + airlines[airlineCode].Flights[ChosenFlightNum].FlightNumber);
        Console.WriteLine("Origin: " + airlines[airlineCode].Flights[ChosenFlightNum].Origin);
        Console.WriteLine("Destination: " + airlines[airlineCode].Flights[ChosenFlightNum].Destination);
        Console.WriteLine("Expected Departure/Arrival Time: " + airlines[airlineCode].Flights[ChosenFlightNum].ExpectedTime);
        Console.WriteLine("Status: " + airlines[airlineCode].Flights[ChosenFlightNum].Status);
        Console.WriteLine("Special Request Code: " + GetSpecialRequestCode(airlines[airlineCode].Flights[ChosenFlightNum]));
        Console.WriteLine($"Boarding Gate: {GetBoardingGateforFlight(airlines[airlineCode].Flights[ChosenFlightNum], boardingGates)}");
    }

    else
    {

        while (true)
        {
            try
            {
                Console.WriteLine("Are you sure you want to delete this flight? (Y/N)");
                string option = Console.ReadLine();
                if (option.ToUpper() == "Y")
                {
                    airlines[airlineCode].RemoveFlight(airlines[airlineCode].Flights[ChosenFlightNum]);
                    flights.Remove(ChosenFlightNum);
                    Console.WriteLine("Flight has been deleted.");
                    break;
                }

                else if (option.ToUpper() == "N")
                {
                    Console.WriteLine("Flight has not been deleted.");
                    break;
                }

                else if (option == "")
                {
                    throw new Exception();
                }

                else
                {
                    throw new Exception();
                }

            }

            catch (Exception)
            {
                Console.WriteLine("Please pick the correct option.");
                continue;
            }
        }

    }
}

void DisplayFlightSchedule(Dictionary<string, Flight> flights, Dictionary<string, Airline> airlines, Dictionary<string, BoardingGate> boardingGates)
{
    Console.WriteLine("=============================================\r\nFlight Schedule for Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time     Status          Boarding Gate");
    List<Flight> flightsList = new List<Flight>();
    foreach (Flight flight in flights.Values)
    {
        flightsList.Add(flight);
    }
    flightsList.Sort();
    foreach (Flight flight in flightsList)
    {
        foreach (Airline airline in airlines.Values)
        {
            string code = flight.FlightNumber.Split(' ')[0];
            if (airline.Code == code)
            {
                Console.WriteLine($"{flight.FlightNumber,-15} {airline.Name,-22} {flight.Origin,-22} {flight.Destination,-22} {flight.ExpectedTime,-33} {flight.Status,-18} {GetBoardingGateforFlight(flight, boardingGates)}");
            }
        }
    }
}

void ProcessUnassignedFlights(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Airline> airlines)
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    List<BoardingGate> unassignedGates = new List<BoardingGate>();
    int assignedFlightsCount = 0;
    int assignedBoardingGatesCount = 0;
    foreach (Flight flight in flights.Values)
    {
        if (GetBoardingGateforFlight(flight, boardingGates) == "Unassigned")
        {
            unassignedFlights.Enqueue(flight);
        }
        else
        {
            assignedFlightsCount++;
        }
    }
    foreach (BoardingGate boardingGate in boardingGates.Values)
    {
        if (boardingGate.Flight == null)
        {
            unassignedGates.Add(boardingGate);
        }
        else
        {
            assignedBoardingGatesCount++;
        }
    }
    Console.WriteLine("Total Number of Boarding Gates that do not have a Flight Number assigned: " + unassignedGates.Count);
    int processedCount = 0;
    while (unassignedFlights.Count > 0)
    {
        Flight flight = unassignedFlights.Dequeue();
        BoardingGate boardingGate = null;
        foreach (BoardingGate gate in unassignedGates)
        {
            if (gate.SupportsCFFT && flight is CFFTFlight)
            {
                boardingGate = gate;
                break;
            }
            else if (gate.SupportsDDJB && flight is DDJBFlight)
            {
                boardingGate = gate;
                break;
            }
            else if (gate.SupportsLWTT && flight is LWTTFlight)
            {
                boardingGate = gate;
                break;
            }
            else if (flight is NORMFlight && (!(gate.SupportsLWTT) && !(gate.SupportsDDJB) && !(gate.SupportsCFFT)))
            {
                boardingGate = gate;
                break;
            }

        }
        boardingGate.Flight = flight;
        Console.WriteLine(flight.FlightNumber + " has been assigned to " + boardingGate.GateName + "!");
        processedCount++;
        unassignedGates.Remove(boardingGate);
        Console.WriteLine("\nDisplaying flight details for " + flight.FlightNumber);
        Console.WriteLine("Flight Number: " + flight.FlightNumber);
        Console.WriteLine("Airline Name: " + airlines[flight.FlightNumber.Split(' ')[0]].Name);
        Console.WriteLine("Origin: " + flight.Origin);
        Console.WriteLine("Destination: " + flight.Destination);
        Console.WriteLine("Expected Time: " + flight.ExpectedTime);
        Console.WriteLine("Status: " + flight.Status);
        Console.WriteLine("Special Request Code: " + GetSpecialRequestCode(flight));
        Console.WriteLine("Boarding Gate: " + GetBoardingGateforFlight(flight, boardingGates));
        Console.WriteLine("\nDisplaying the next boarding gate...");
    }
    Console.WriteLine("\nTotal Number of Flights that have been assigned to a Boarding Gate: " + processedCount);
    Console.WriteLine($"Total Boarding Gates Processed and Assigned: {processedCount}");
    double percentageAutomaticallyProcessedFlights = ((double)processedCount - assignedFlightsCount) / flights.Count * 100;
    double percentageAutomaticallyProcessedGates = ((double)processedCount - assignedBoardingGatesCount) / boardingGates.Count * 100;
    Console.WriteLine($"Percentage of Flights Automatically Processed: {percentageAutomaticallyProcessedFlights:F2}%");
    Console.WriteLine($"Percentage of Boarding Gates Automatically Processed: {percentageAutomaticallyProcessedGates:F2}%");
}

void DisplayAirlineFee(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates)
{
    bool check = false;
    foreach (Flight flight in flights.Values)
    {
        if (GetBoardingGateforFlight(flight, boardingGates) == "Unassigned")
        {
            check = true;
        }
    }
    if (check == true)
    {
        Console.WriteLine("There are unassigned flights. Please assign all flights to boarding gates before calculating the fees.");

    }
    else
    {
        Console.WriteLine("=============================================\r\nAirline Fees for Changi Airport Terminal 5\r\n=============================================\r\n");
        terminal5.PrintAirlineFees();
    }

}

