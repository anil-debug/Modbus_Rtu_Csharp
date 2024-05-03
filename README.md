# ModbusCommunicator

The ModbusCommunicator is a C# class designed to facilitate communication with Modbus slave devices using the NModbus library.

## Prerequisites

Before using the ModbusCommunicator class, ensure you have the following prerequisites:

- **.NET Framework**: The class is designed to work with .NET Framework. Make sure you have .NET Framework installed on your system.

- **NModbus Library**: This class relies on the NModbus library for Modbus communication. You can install the library using NuGet Package Manager.

- **Serial Port**: Ensure that your system has a serial port available and accessible. The class communicates with Modbus slave devices via a serial port. Make sure the correct serial port (RS-232, RS-422, RS-485, etc.) is used as per your device's specifications.

## Usage

1. **Integration**: Add the ModbusCommunicator.cs file to your C# project.

2. **Initialization**: Create an instance of ModbusCommunicator with the desired communication parameters (port name, baud rate, parity, timeout).

3. **Connecting to Modbus Slave**: Call the Connect() method to establish a connection with the Modbus slave.

4. **Reading Holding Registers**: Use the ReadHoldingRegisters() method to read data from holding registers of the Modbus slave.

5. **Writing Holding Registers**: Use the WriteHoldingRegisters() method to write data to holding registers of the Modbus slave.

6. **Closing Connection**: Call the Close() method to close the connection with the Modbus slave.

## Example

Here's a simple example demonstrating how to use the ModbusCommunicator class:

```csharp
// Initialize ModbusCommunicator
ModbusCommunicator communicator = new ModbusCommunicator("/dev/ttyUSB0", 9600, Parity.Odd, 1000);

// Connect to Modbus slave device
bool connectionStatus = communicator.Connect();

if (connectionStatus)
{
    // Read and write operations
    // ...

    // Close connection
    communicator.Close();
}
else
{
    Console.WriteLine("Connection failed. Please check the communication settings.");
}
