import serial
import time
port = '/dev/cu.usbmodem14101'
ser = serial.Serial(port, 9600)

def physicalread():
    s = ser.read(10000)
    return s

def physicalWrite(command):
    ser.write(bytes(command, 'utf-8'))