# ---------------------
#   TeamSpeak-3 Bot
# kerbalspaceprogram.de
#  Thomas P. und KCST
# ---------------------

# Imports
import datetime
import shutil
import enum
import os

# Constants
loggingDirectory = 'logs/'
lastLoggedDay = int(datetime.datetime.strftime(datetime.datetime.utcnow(), '%d'))

# --
# Logging Module to log messages to file and CLI
# --

# Check the directory
if not os.path.exists(loggingDirectory):
    os.makedirs(loggingDirectory, 777, True)

# Open the log
file = open(loggingDirectory + 'latest.log', 'a+', -1, 'utf-8', None, '\n', )

# Logging
def log(messsage, level : str):
    
    # Globals
    global lastLoggedDay

    # Check for new day
    if datetime.datetime.day == lastLoggedDay + 1:
        newDay()

    # Log
    file.writelines('[' + str(level) + ' ' + datetime.datetime.strftime(datetime.datetime.utcnow(), "%H:%M:%S") + ']: ' + messsage + '\n')
    print('[' + str(level) + ' ' + datetime.datetime.strftime(datetime.datetime.utcnow(), "%H:%M:%S") + ']: ' + messsage)

    # Set lastLoggedDay
    lastLoggedDay = int(datetime.datetime.strftime(datetime.datetime.utcnow(), '%d'))

# Moves the current file to a different name and creates a new one
def newDay():
    
    # Globals
    global file

    # Get the new filename
    name = datetime.datetime.strftime(datetime.datetime.utcnow(), '%Y-%m') + '-' + lastLoggedDay + '.log';

    # Close the old file
    file.close()

    # Move it away
    shutil.move(loggingDirectory + 'latest.log', name)

    # Open a new file
    file = open(loggingDirectory + 'latest.log', 'a+', -1, 'utf-8', None, '\n')

# Class to store logging levels
class level:
    DEBUG = 'DEBUG'
    INFO = 'INFO'
    WARNING = 'WARNING'
    ERROR = 'ERROR'
    SPECIAL = 'SPECIAL'
    VERYSPECIAL = 'VERYSPECIAL'