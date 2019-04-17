import random
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_animation import *


class DeusExMachina(QLabel):
    x = 0
    y = 0
    width = 20
    height = 20
    active = False

    def __init__(self, x, y, parent):
        super().__init__(parent)

        self.x = x
        self.y = y

        self.setGeometry(self.x, self.y, self.width, self.height)

        self.animacija = ImageAnimation('resources/deus_ex_machina.png', self.width, self.height, self)
        self.animacija._update(0, 2)
        self.animacija.play()

        self.xd = 0
        self.yd = 0

        self.show()


    def closeEvent(self, event):
        del self.animacija








