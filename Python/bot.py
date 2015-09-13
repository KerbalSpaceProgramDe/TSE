# ----------------
# TeamSpeak³ - Bot
# Thomas P. - KCST
# www.kerbal.de
# encoding: utf-8
# ----------------

# Import
import logger as logging, settings, colorer, ts3, globals, time

# Hallo sagen
logging.debug("Hallo, ich bin ein TeamSpeak³-Bot.")

# Status
globals.isRunning = True

# Zum TS3 Server Verbinden
logging.info("Baue Verbindung zu " + settings.adress + ":" + str(settings.query) + " auf...")
ts3conn = None;
try:
    ts3conn = ts3.query.TS3Connection(settings.adress, settings.query)
except:
    logging.error("Verbindung fehlgeschlagen!")
    exit()
logging.debug("Verbindung erfolgreich!")

# Einloggen
logging.info("Starte Authentifizierung...")
try:
    ts3conn.login(client_login_name=settings.username, client_login_password=settings.password)
except:
    logging.error("Authentifizierung fehlgeschlagen!")
    exit()
logging.debug("Authentifizierung erfolgreich!")

# Nicht ausgehen
while (globals.isRunning):
    # Hier Bot-Logik implementieren!
    # Eventuell Kommando-System?
    time.sleep(0.1)
    continue

# Alles ausschalten etc.