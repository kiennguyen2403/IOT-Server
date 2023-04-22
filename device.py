import serial
import time
port = 'COM5'
ser = serial.Serial(port, 9600)

def physicalRead():
    s = ser.read(10000)
    return s

def physicalWrite(command):
    ser.write(bytes(command, 'utf-8'))