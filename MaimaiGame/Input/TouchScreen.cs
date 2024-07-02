using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MaimaiGame.Input;

public class TouchScreen
{
	public readonly Dictionary<TouchScreenSensor, bool> Sensors;

	public EventHandler<InputReceivedEventArgs>? InputReceived;

	private readonly SerialPort _serialPort;

	public TouchScreen(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
	{
		Sensors = new Dictionary<TouchScreenSensor, bool>();

		foreach (TouchScreenSensor sensor in Enum.GetValues<TouchScreenSensor>())
			Sensors.Add(sensor, false);

		_serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
		_serialPort.DataReceived += OnDataReceived;

		try
		{
			_serialPort.Open();

			byte[] readCommand = Encoding.UTF8.GetBytes("{RSAT}");
			_serialPort.Write(readCommand, 0, readCommand.Length);
		}
		catch (Exception e)
		{
			Logger.Error(e);
		}
	}

	private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		byte[] buffer = new byte[9];
		int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

		if (bytesRead < 9)
			return;

		bool change = false;
		byte bit = 1;

		for (int i = 0; i < Sensors.Count; i++)
		{
			TouchScreenSensor sensor = (TouchScreenSensor) i;
			bool state = (buffer[i / 5 + 1] & bit) != 0;

			if (state != Sensors[sensor])
				change = true;

			Sensors[sensor] = state;

			if (bit == 16)
				bit = 1;
			else
				bit <<= 1;
		}

		if (change)
			InputReceived?.Invoke(this, new InputReceivedEventArgs());
	}
}

public enum TouchScreenSensor
{
	A1,
	A2,
	A3,
	A4,
	A5,
	A6,
	A7,
	A8,
	B1,
	B2,
	B3,
	B4,
	B5,
	B6,
	B7,
	B8,
	C1,
	C2,
	D1,
	D2,
	D3,
	D4,
	D5,
	D6,
	D7,
	D8,
	E1,
	E2,
	E3,
	E4,
	E5,
	E6,
	E7,
	E8
}

public class InputReceivedEventArgs : EventArgs { }