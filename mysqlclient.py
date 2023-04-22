import mysqlclient
import mysql.connector

mydb = mysql.connector.connect(
    host="localhost",
    user="yourusername",
    password="yourpassword"
)


def create():
    mycursor = mydb.cursor()
    mycursor.execute("CREATE DATABASE mydatabase")

def drop():
    mycursor = mydb.cursor()
    mycursor.execute("DROP DATABASE mydatabase")

def createTable():
    mycursor = mydb.cursor()
    mycursor.execute("CREATE TABLE customers (name VARCHAR(255), address VARCHAR(255))")

def dropTable():
    mycursor = mydb.cursor()
    mycursor.execute("DROP TABLE customers")

def insert():
    mycursor = mydb.cursor()
    sql = "INSERT INTO customers (name, address) VALUES (%s, %s)"
    val = ("John", "Highway 21")
    mycursor.execute(sql, val)
    mydb.commit()
    print(mycursor.rowcount, "record inserted.")

def select():
    mycursor = mydb.cursor()
    mycursor.execute("SELECT * FROM customers")
    myresult = mycursor.fetchall()
    for x in myresult:
        print(x)