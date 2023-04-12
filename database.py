import sqlite3
connection = sqlite3.connect('database.db')

def create():
    with open('./sql/schema.sql') as f:
        connection.executescript(f.read())
    cur = connection.cursor()
    cur.execute("INSERT INTO devices (device) VALUES (?)",
            ('Bedroom Led',)
            )

    cur.execute("INSERT INTO devices (device) VALUES (?)",
            ('Livingroom Led',)
            )
    connection.commit()
    connection.close()

def drop():
    with open('./sql/drop.sql') as f:
        connection.executescript(f.read())
    connection.commit()
    connection.close()

def getAll():
    cur = connection.cursor()
    data = cur.execute("SELECT * FROM posts")
    connection.commit()
    connection.close()
    return data

def getID(id):
    cur = connection.cursor()
    data = cur.execute("SELECT * FROM posts WHERE device=?",(id,))
    connection.commit()
    connection.close()
