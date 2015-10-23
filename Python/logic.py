# ----------------
# TeamSpeak³ - Bot
# Thomas P. - KCST
# www.kerbal.de
# encoding: utf-8
# ----------------

# Import
import logger as logging, settings, colorer, ts3, globals

# Liste mit allen Funktionen
functions = []

# Attribut, um Funktionen zu erkennen
def logic(function):
    functions.append(function)
    return function

### LOGIC IMPLEMENTATION -- START ###

# Variablen
tilLastPoke = 0

@logic # Test-Implementation des Logik Systems - Stupst mich (Thomas) alle 120 frames an :D - Killt den Bot wenn ich nicht auf dem Server bin *pfeif*
def Poke(ts3conn):
    global tilLastPoke
    if tilLastPoke == 0:
        clid = ts3conn.clientfind(pattern = "Thomas P.")[0]['clid']
        ts3conn.clientpoke(msg = "Hallo", clid = clid)
        tilLastPoke = 120
    tilLastPoke -= 1

### LOGIC IMPLEMENTATION -- STOP ###

# Alle Logic Funktionen durchlaufen
def Run(ts3conn):
    for function in functions:
        function(ts3conn)