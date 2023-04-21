import sqlite3
connection = sqlite3.connect('database.db', check_same_thread=False)

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
    # connection.close()

def getStatus():
    cur = connection.cursor()
    data = cur.execute("SELECT * FROM devices").fetchone()
    connection.commit()
    # connection.close()
    return data

def getAll():
    cur = connection.cursor()
    data = cur.execute("SELECT * FROM posts").fetchall()
    connection.commit()
    # connection.close()
    return data

def getID(id):
    cur = connection.cursor()
    data = cur.execute("SELECT * FROM posts WHERE device=?",(id,)).fetchone()
    connection.commit()
    return data
    # connection.close()

def insert(command, device):
    cur = connection.cursor()
    cur.execute("INSERT INTO posts (command, device) VALUES (?,?)",
            (command, device)
            )
    connection.commit()
    print("Insert success")
    # connection.close()