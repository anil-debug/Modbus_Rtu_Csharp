using System;
using System.IO.Ports;
using NModbus;
using NModbus.Serial;

public class ModbusCommunicator
{
    private SerialPort port;            // Serial port for communication
    private IModbusSerialMaster master; // Modbus master for communication

    // Constructor to initialize the Modbus communicator
    public ModbusCommunicator(string portName = "/dev/ttyUSB0", int baudRate = 9600, Parity parity = Parity.Odd, int timeout = 1000)
    {
        // Create a new serial port with specified parameters
        port = new SerialPort(portName, baudRate, parity);
        port.ReadTimeout = timeout;
        port.WriteTimeout = timeout;

        // Create a new instance of ModbusFactory to create Modbus master
        var factory = new ModbusFactory();

        // Create RTU transport using the serial port
        var transport = factory.CreateRtuTransport(port);

        // Create Modbus master using the RTU transport
        master = factory.CreateMaster(transport);
    }

    // Method to connect to the Modbus slave device
    public bool Connect()
    {
        try
        {
            // Open the serial port if it's not already open
            if (!port.IsOpen)
                port.Open();
            return true; // Return true if connection is successful
        }
        catch (Exception e)
        {
            // Handle connection failure and print error message
            Console.WriteLine("Connection failed: " + e.Message);
            return false; // Return false if connection fails
        }
    }

    // Method to read holding registers from the Modbus slave device
    public ushort[] ReadHoldingRegisters(ushort startAddress, ushort count, byte slaveId = 1)
    {
        try
        {
            // Read holding registers from the Modbus slave using the Modbus master
            return master.ReadHoldingRegisters(slaveId, startAddress, count);
        }
        catch (Exception e)
        {
            // Handle read error and print error message
            Console.WriteLine("An error occurred during Modbus read: " + e.Message);
            throw; // Re-throw the caught exception to propagate it to the caller
        }
    }

    // Method to write holding registers to the Modbus slave device
    public bool WriteHoldingRegisters(ushort startAddress, ushort[] data, byte slaveId = 1)
    {
        try
        {
            // Write holding registers to the Modbus slave using the Modbus master
            master.WriteMultipleRegisters(slaveId, startAddress, data);
            
            // Print success message with the written data
            Console.WriteLine("Data written to holding registers: " + string.Join(", ", data));
            return true; // Return true if write operation is successful
        }
        catch (Exception e)
        {
            // Handle write error and print error message
            Console.WriteLine("An error occurred during Modbus write: " + e.Message);
            return false; // Return false if write operation fails
        }
    }

    // Method to close the connection to the Modbus slave device
    public void Close()
    {
        try
        {
            // Close the serial port
            port.Close();
            Console.WriteLine("Connection closed"); // Print success message
        }
        catch (Exception e)
        {
            // Handle closing error and print error message
            Console.WriteLine("Error occurred while closing connection: " + e.Message);
        }
    }

    // Main method for testing the Modbus communication
    static void Main(string[] args)
    {
        // Create a Modbus communicator instance with default parameters
        ModbusCommunicator communicator = new ModbusCommunicator("/dev/ttyUSB0", 9600, Parity.Odd, 1000);

        // Connect to the Modbus slave device
        bool connectionStatus = communicator.Connect();

        // Check if connection is successful
        if (connectionStatus)
        {
            Console.WriteLine("Connected to Modbus slave device!");

            ushort[] writeData = new ushort[] { 10, 20, 30, 0, 1 }; // Example data

            // Write data to holding registers of the Modbus slave
            bool writeSuccess = communicator.WriteHoldingRegisters(114, writeData, 1);

            // Check if write operation is successful
            if (writeSuccess)
            {
                Console.WriteLine("Data written successfully to holding registers!");

                // Read data from holding registers of the Modbus slave
                ushort[] readData = communicator.ReadHoldingRegisters(114, (ushort)writeData.Length);

                // Check if read operation is successful
                if (readData != null)
                {
                    Console.WriteLine("Data read successfully from holding registers: " + string.Join(", ", readData));
                }
                else
                {
                    Console.WriteLine("Failed to read data from holding registers after write operation.");
                }
            }
            else
            {
                Console.WriteLine("Write operation failed. Check for errors.");
            }

            // Close the connection to the Modbus slave device
            communicator.Close();
        }
        else
        {
            Console.WriteLine("Connection failed. Please check the communication settings.");
        }
    }
}
