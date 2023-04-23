from device import physicalRead, openSerial
import requests

print("Start monitoring")
ser = openSerial()
current = ""
while True:
    response = physicalRead(ser)
    print(response)
    if response != current:
        if (response == "livingroom1"):
            requests.post("http://localhost:5000/status/1", json={"led": 0, "data": "on"})
        elif (response == "livingroom0"):
            requests.post("http://localhost:5000/status/1", json={"led": 0, "data": "off"})
        elif (response == "bedroom1"):
            requests.post("http://localhost:5000/status/1", json={"led": 1, "data": "on"})
        elif (response == "bedroom0"):
            requests.post("http://localhost:5000/status/1", json={"led": 1, "data": "off"})
