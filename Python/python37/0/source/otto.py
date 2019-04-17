import random
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_animation import *
from source.Bullet import *


class Otto(QLabel):
    x = 0
    y = 0
    width = 8
    height = 8
    death = False

    def __init__(self, x, y, chase, parent):
        super().__init__(parent)

        self.x = x
        self.y = y

        self.setGeometry(self.x, self.y, self.width, self.height)

        self.animacija = ImageAnimation('resources/otto.png', self.width, self.height, self)
        self.animacija._update(0, 5)
        self.animacija.play()

        self.steps = 0
        self.xd = 0
        self.yd = 0
        self.chase = chase

        self.show()

    def _move(self):
        self.x += self.xd
        self.y += self.yd
        self.setGeometry(self.x, self.y, self.width, self.height)


    def closeEvent(self, event):
        del self.animacija








