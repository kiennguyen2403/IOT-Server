import mysqlclient
import mysql.connector

mydb = mysql.connector.connect(
    host="localhost",
    user="root",
    password="."
)

mydb.autocommit = True

def create():
    mycursor = mydb.cursor()
    mycursor.execute("CREATE DATABASE database")
    mycursor.execute("USE database")
    mycursor.execute("CREATE TABLE devices (id INT AUTO_INCREMENT PRIMARY KEY, device VARCHAR(255))")
    mycursor.execute("CREATE TABLE posts (id INT AUTO_INCREMENT PRIMARY KEY, command VARCHAR(255), device INT, created TIMESTAMP DEFAULT CURRENT_TIMESTAMP)")
    mycursor.execute("INSERT INTO devices (device) VALUES (%s)",
                     ('Bedroom Led',))
    mycursor.execute("INSERT INTO devices (device) VALUES (%s)",
                     ('Livingroom Led',))
    mycursor.execute("INSERT INTO posts (command, device) VALUES (off,0)")
    mycursor.execute("INSERT INTO posts (command, device) VALUES (off,1)")
    mydb.close()
    
def drop():
    mycursor = mydb.cursor()
    mycursor.execute("DROP DATABASE database")
    mydb.close()


def insert(command, device):
    mycursor = mydb.cursor()
    sql = "INSERT INTO posts (command, device) VALUES (%s, %s)"
    val = (command, device)
    mycursor.execute(sql, val)
    mydb.commit()
    print(mycursor.rowcount, "record inserted.")

def getAll():
    mycursor = mydb.cursor()
    mycursor.execute("SELECT * FROM posts")
    myresult = mycursor.fetchall()
    return myresult

def getID(id):
    mycursor = mydb.cursor()
    sql = "SELECT * FROM posts WHERE device = %s ORDER BY created DESC LIMIT 1"
    adr = (id,)
    mycursor.execute(sql, adr)
    myresult = mycursor.fetchone()
    return myresult

create()
