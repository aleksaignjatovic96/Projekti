import sys

from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *
from PyQt5.QtCore import Qt

from source.level import *
from source.player import *
from source.enemy import *


class ScoreWindow(QWidget):
    scorelist = {}

    def __init__(self):
        super().__init__()

        self.setAutoFillBackground(True)
        p = self.palette()
        p.setColor(self.backgroundRole(), Qt.black)
        self.setPalette(p)

        self.label_array = ['1.', '2.', "3.", "4.", "5."]

        font = QFont()
        font.setFamily(QFontDatabase.applicationFontFamilies(0)[0])
        font.setStyleStrategy(QFont.PreferQuality)
        font.setHintingPreference(QFont.PreferFullHinting)
        font.setPixelSize(23)
        font.setWeight(QFont.Normal)

        self.header = QLabel(self)
        self.header.setAlignment(Qt.AlignLeft)
        self.header.setGeometry(250, 30, 400, 130)
        self.header.setText("== S C O R E ==")
        self.header.setFont(font)
        self.header.setStyleSheet('color: yellow')

        self.label_score1 = QLabel(self)
        self.label_score1.setAlignment(Qt.AlignLeft)
        self.label_score1.setGeometry(100, 70, 550, 100)
        self.label_score1.setText(self.label_array[0])
        self.label_score1.setFont(font)
        self.label_score1.setStyleSheet('color: yellow')

        self.label_score2 = QLabel(self)
        self.label_score2.setAlignment(Qt.AlignLeft)
        self.label_score2.setGeometry(100, 100, 550, 130)
        self.label_score2.setText(self.label_array[1])
        self.label_score2.setFont(font)
        self.label_score2.setStyleSheet('color: yellow')

        self.label_score3 = QLabel(self)
        self.label_score3.setAlignment(Qt.AlignLeft)
        self.label_score3.setGeometry(100, 130, 550, 160)
        self.label_score3.setText(self.label_array[2])
        self.label_score3.setFont(font)
        self.label_score3.setStyleSheet('color: yellow')

        self.label_score4 = QLabel(self)
        self.label_score4.setAlignment(Qt.AlignLeft)
        self.label_score4.setGeometry(100, 160, 550, 190)
        self.label_score4.setText(self.label_array[3])
        self.label_score4.setFont(font)
        self.label_score4.setStyleSheet('color: yellow')

        self.label_score5 = QLabel(self)
        self.label_score5.setAlignment(Qt.AlignLeft)
        self.label_score5.setGeometry(100, 190, 550, 220)
        self.label_score5.setText(self.label_array[4])
        self.label_score5.setFont(font)
        self.label_score5.setStyleSheet('color: yellow')

        self.label_input = QLineEdit(self)
        self.label_input.setGeometry(130, 230, 250, 30)
        self.label_input.setMaxLength(20)
        self.label_input.setAlignment(Qt.AlignLeft)
        self.label_input.setFont(font)
        self.label_input.setStyleSheet('color: yellow;background-color: black; ')
        self.label_input.returnPressed.connect(self._writeScore)
        self.label_input.hide()

        self._loadScore()
        self.show()

    def _writeScore(self):
        print("Write")

        self.label_input.hide()

        self.scorelist[self.label_input.text()] = self.scorelist.pop("Player")

        with open("highscores.txt", "w") as f:
            for idx, (name, pts) in enumerate(sorted(self.scorelist.items(), key=lambda x: -x[1])):
                f.write(f"{name}:{pts}\n")
                if idx == 4:
                    break

        self._loadScore()
        self._showScore()

    def _loadScore(self):
        self.scorelist = {}
        with open("highscores.txt", "r") as f:
            for line in f:
                name, _, points = line.partition(":")
                if name and points:
                    self.scorelist[name] = int(points)

    def _showScore(self):

        i = 0
        for k, v in self.scorelist.items():
            print(k, v)
            self.label_array[i] = "{0}. {1:9}                      {blc}".format(i + 1, k, blc=v)
            i += 1

        self.label_score1.setText(self.label_array[0])
        self.label_score2.setText(self.label_array[1])
        self.label_score3.setText(self.label_array[2])
        self.label_score4.setText(self.label_array[3])
        self.label_score5.setText(self.label_array[4])

    def _newScore(self, name, score):

        newscore = False

        if len(self.scorelist) < 5:
            newscore = True

        for k, v in self.scorelist.items():
            if score >= v:
                newscore = True
                break

        if newscore == True:
            self.scorelist[name] = score
            self.label_input.setText(name)
            self.label_input.setFocus()
            self.label_input.show()


