﻿# ----------------
# TeamSpeak³ - Bot
# Thomas P. - KCST
# www.kerbal.de
# encoding: utf-8
# ----------------

# Import
import logging, platform

# Farb-Handler Windows
def color_windows(fn):
    def _out_handle(self):
        import ctypes
        return ctypes.windll.kernel32.GetStdHandle(self.STD_OUTPUT_HANDLE)
    out_handle = property(_out_handle)

    def _set_color(self, code):
        import ctypes
        self.STD_OUTPUT_HANDLE = -11
        hdl = ctypes.windll.kernel32.GetStdHandle(self.STD_OUTPUT_HANDLE)
        ctypes.windll.kernel32.SetConsoleTextAttribute(hdl, code)
    setattr(logging.StreamHandler, '_set_color', _set_color)

    def new(*args):
        FOREGROUND_BLUE      = 0x0001
        FOREGROUND_GREEN     = 0x0002
        FOREGROUND_RED       = 0x0004
        FOREGROUND_INTENSITY = 0x0008
        FOREGROUND_WHITE     = FOREGROUND_BLUE|FOREGROUND_GREEN |FOREGROUND_RED
        STD_INPUT_HANDLE = -10
        STD_OUTPUT_HANDLE = -11
        STD_ERROR_HANDLE = -12
        FOREGROUND_BLACK     = 0x0000
        FOREGROUND_BLUE      = 0x0001
        FOREGROUND_GREEN     = 0x0002
        FOREGROUND_CYAN      = 0x0003
        FOREGROUND_RED       = 0x0004
        FOREGROUND_MAGENTA   = 0x0005
        FOREGROUND_YELLOW    = 0x0006
        FOREGROUND_GREY      = 0x0007
        FOREGROUND_INTENSITY = 0x0008
        BACKGROUND_BLACK     = 0x0000
        BACKGROUND_BLUE      = 0x0010
        BACKGROUND_GREEN     = 0x0020
        BACKGROUND_CYAN      = 0x0030
        BACKGROUND_RED       = 0x0040
        BACKGROUND_MAGENTA   = 0x0050
        BACKGROUND_YELLOW    = 0x0060
        BACKGROUND_GREY      = 0x0070
        BACKGROUND_INTENSITY = 0x0080

        levelno = args[1].levelno
        if(levelno>=50):
            color = FOREGROUND_MAGENTA#BACKGROUND_YELLOW | FOREGROUND_RED | FOREGROUND_INTENSITY | BACKGROUND_INTENSITY 
        elif(levelno>=40):
            color = FOREGROUND_RED# | FOREGROUND_INTENSITY
        elif(levelno>=30):
            color = FOREGROUND_YELLOW# | FOREGROUND_INTENSITY
        elif(levelno>=20):
            color = FOREGROUND_WHITE
        elif(levelno>=10):
            color = FOREGROUND_GREEN
        else:
            color =  FOREGROUND_WHITE
        args[0]._set_color(color)

        ret = fn(*args)
        args[0]._set_color( FOREGROUND_WHITE )
        return ret
    return new

# Farb-Handler MacOS/*nix
def color_ansi(fn):
    def new(*args):
        levelno = args[1].levelno
        if(levelno>=50):
            color = '\x1b[35m'
        elif(levelno>=40):
            color = '\x1b[31m'
        elif(levelno>=30):
            color = '\x1b[33m'
        elif(levelno>=20):
            color = '\x1b[0m'
        elif(levelno>=10):
            color = '\x1b[32m'
        else:
            color = '\x1b[0m'
        args[1].msg = color + args[1].msg +  '\x1b[0m'
        return fn(*args)
    return new


if platform.system()=='Windows':
    logging.StreamHandler.emit = color_windows(logging.StreamHandler.emit)
else:
    logging.StreamHandler.emit = color_ansi(logging.StreamHandler.emit)