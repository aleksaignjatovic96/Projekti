import sys

from PyQt5.QtWidgets import *
from PyQt5.QtCore import *
from PyQt5.QtGui import *

from source.berzerk import *

def main():

    QApplication.setAttribute(Qt.AA_EnableHighDpiScaling) #set scaling attribute
    QApplication.setAttribute(Qt.AA_UseHighDpiPixmaps) #set high dpi icon
    app = QApplication(sys.argv) #create new application

    berzerk = Berzerk() #create new instance of main window
    berzerk.show() #make instance visible
    berzerk.raise_() #raise instance to top of window stack
    sys.exit(app.exec_()) #monitor application for events

if __name__ == '__main__':
    main()
