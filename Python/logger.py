# ----------------
# TeamSpeak³ - Bot
# Thomas P. - KCST
# www.kerbal.de
# encoding: utf-8
# ----------------

# Import
import logging, colorer, settings #Hier ist logging noch dass was es ist, nirgendwo sonst! Warum? Wen juckts :D
from encodings import utf_8
from datetime import datetime

# Init
logging.basicConfig(format='%(message)s', level=logging.DEBUG)
file = open(settings.logfile, "w", -1, "utf_8", None, "\n", True, None)

# Info
def info(msg):
    msg = "[" + time() + "] " + msg
    logging.info(msg)
    toFile(msg)

# Warning
def warning(msg):
    msg = "[" + time() + "] " + msg
    logging.warning(msg)
    toFile(msg)

# Error
def error(msg):
    msg = "[" + time() + "] " + msg
    logging.error(msg)
    toFile(msg)

# Debug
def debug(msg):
    msg = "[" + time() + "] " + msg
    logging.debug(msg)
    toFile(msg)

# Time
def time():
    return datetime.strftime(datetime.now(), "%H:%M:%S")

# Log-to-File
def toFile(msg):
    file.writelines(msg + "\n")
    file.flush()    
