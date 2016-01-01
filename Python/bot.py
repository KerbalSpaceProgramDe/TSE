# ---------------------
#   TeamSpeak-3 Bot
# kerbalspaceprogram.de
#  Thomas P. und KCST
# ---------------------

# Imports
import ts3
import logger
import json
import os
import internal
import time

# Constants
settingsDirectory = 'settings/'
dataDirectory = 'data/'
botSettingsName = 'bot.json'
connSettingsName = 'ts.json'

# --
# Main Bot Module. Here is everything stored that is related to the TeamSpeak Connection
# --

# Check the directories
if not os.path.exists(settingsDirectory):
    os.makedirs(settingsDirectory, 777, True)
if not os.path.exists(dataDirectory):
    os.makedirs(dataDirectory, 777, True)

# Load the bot settings
if not os.path.isfile(settingsDirectory + botSettingsName):
    json.dump(internal.prefabBotSettings, open(settingsDirectory + botSettingsName, 'w', -1, 'utf-8', None, '\n'), skipkeys=True, ensure_ascii=False, indent=4, sort_keys=True) 
bot = json.load(open(settingsDirectory + botSettingsName, 'r', -1, 'utf-8', None, '\n'))

# Load the connection settings
if not os.path.isfile(settingsDirectory + connSettingsName):
    json.dump(internal.prefabConnSettings, open(settingsDirectory + connSettingsName, 'w', -1, 'utf-8', None, '\n'), skipkeys=True, ensure_ascii=False, indent=4, sort_keys=True) 
connection = json.load(open(settingsDirectory + connSettingsName, 'r', -1, 'utf-8', None, '\n'))

# Say hello
logger.log('TeamSpeak-E - A TeamSpeak Management Bot by kerbal.de', logger.level.SPECIAL)

# TS3 Connection
ts = None

# Use a function for this so that we can reconnect
def Connect():
    global ts
    try:
        # Create the connection to the Server
        logger.log('Connecting to ' + connection['host'] + ':' + str(connection['port']) + '. Pending...', logger.level.INFO)
        ts = ts3.query.TS3Connection(host=connection['host'], port=connection['port'])

        # Wait for connected
        while not ts.is_connected(): continue

        # Connected
        logger.log('Connection to TeamSpeak Server established!', logger.level.INFO)
        logger.log('Sending User Information...', logger.level.INFO)

        # Send User Information
        ts.login(client_login_name=connection['name'], client_login_password=connection['password'])
        logger.log('Logged into TeamSpeak Server.', logger.level.INFO)

        # Select Server
        ts.use(sid=connection['id'])
    except ts3.query.TS3Error as error:
        logger.log('TS3Error! Exception: ' + str(error), logger.level.ERROR)
        return
    except ts3.query.TS3QueryError as queryError:
        logger.log('TS3QueryError! Exception: ' + str(queryError), logger.level.ERROR)
        return

# Connect
Connect()

# Don't shutdown
try:
    while True:
        # Do something here
        time.sleep(0.5)
except KeyboardInterrupt:
    # Close the TeamSpeak Connection
    ts.close()
    logger.log('Closed Connection to TeamSpeak Server!', logger.level.DEBUG)
finally:
    # Save things
    json.dump(bot, open(settingsDirectory + botSettingsName, 'w', -1, 'utf-8', None, '\n'), skipkeys=True, ensure_ascii=False, indent=4, sort_keys=True) 
    json.dump(connection, open(settingsDirectory + connSettingsName, 'w', -1, 'utf-8', None, '\n'), skipkeys=True, ensure_ascii=False, indent=4, sort_keys=True) 

    # Save data
    # Implement Data here

    # Bye!
    logger.log('Stopping the Bot Process!', logger.level.DEBUG)