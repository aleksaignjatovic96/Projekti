import sys

from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *
from PyQt5.QtCore import Qt

from source.level import *
from source.player import *
from source.enemy import *

class GameWindow(QWidget):
 
    player_array = []
    main_level = None	
    	
	
    def __init__(self):
        super().__init__()

        self.setAutoFillBackground(True)
        p = self.palette()
        p.setColor(self.backgroundRole(), Qt.black)	
        self.setPalette(p)	
		
        self.font = QFont()
        self.font.setFamily(QFontDatabase.applicationFontFamilies(0)[0])
        self.font.setStyleStrategy(QFont.PreferQuality)
        self.font.setHintingPreference(QFont.PreferFullHinting)
        self.font.setPixelSize(23)
        self.font.setWeight(QFont.Normal)


        self.player_life_label = QLabel(self)
        self.player_life_label.setAlignment(Qt.AlignLeft)
        self.player_life_label.setGeometry(10, 430, 100, 130)
        self.player_life_label.setText("Life")		
        self.player_life_label.setFont(self.font)
        self.player_life_label.setStyleSheet('color: yellow')
		
        self.player_score_label = QLabel(self)
        self.player_score_label.setFixedWidth(640)	
        self.player_score_label.move(-170, 430)
        self.player_score_label.setAlignment(Qt.AlignCenter)
        self.player_score_label.setText("0")		
        self.player_score_label.setFont(self.font)
        self.player_score_label.setStyleSheet('color: yellow')

        self.player_life_label2 = QLabel(self)
        self.player_life_label2.setAlignment(Qt.AlignLeft)
        self.player_life_label2.setGeometry(555, 430, 100, 130)
        self.player_life_label2.setText("Life")
        self.player_life_label2.setFont(self.font)
        self.player_life_label2.setStyleSheet('color: #1ec131')

        self.player_life_label2.hide()

        self.player_score_label2 = QLabel(self)
        self.player_score_label2.setFixedWidth(640)
        self.player_score_label2.move(170, 430)
        self.player_score_label2.setAlignment(Qt.AlignCenter)
        self.player_score_label2.setText("0")
        self.player_score_label2.setFont(self.font)
        self.player_score_label2.setStyleSheet('color: #1ec131')

        self.player_score_label2.hide()
		
        self.show()

    def game_new(self, num_players):
	
        #create Player
        self.player_array = []

        if num_players > 1:
            self.player_life_label2.show()
            self.player_score_label2.show()

        for x in range(num_players):
            self.player_add(x)
            if x == 0:
                self.player_life_label.setText("Life: {0}".format(self.player_array[x].player_life)	)
                self.player_score_label.setText("{0}".format(self.player_array[x].player_score)	)
                self.player_array[x].player_ind = "player1"
            elif x == 1:
                self.player_life_label2.setText("Life: {0}".format(self.player_array[x].player_life))
                self.player_score_label2.setText("{0}".format(self.player_array[x].player_score))
                self.player_array[x].player_ind = "player2"
		
        self.main_level = Level(self)
        self.level_new(num_players)
	
    def level_new(self, num_players):
		
        self.main_level = Level(self)	

        self.main_level.door_add()		
        self.main_level.walls_add()
        self.main_level.maze_add()	
        self.main_level.enemy_add()
        self.main_level.update()	

        for x in range(num_players):
            if self.player_array[x].player_ind == "player1":
                self.player_array[x]._position(30, (400 // 2), "-")
                self.player_array[x]._live()
                self.player_life_label.setText("Life: {0}".format(self.player_array[x].player_life))
                self.player_score_label.setText("{0}".format(self.player_array[x].player_score))
            elif self.player_array[x].player_ind == "player2":
                self.player_array[x]._position(590, (400 //2), "-")
                self.player_array[x]._live()
                self.player_life_label2.setText("Life: {0}".format(self.player_array[x].player_life))
                self.player_score_label2.setText("{0}".format(self.player_array[x].player_score))



    def level_clear(self):
        self.main_level.close() 
        del self.main_level	
		
    def player_add(self, x):

        if x == 0:
            self.player_array.append(Player(30,  (400//2), x, self))

        if x == 1:
            self.player_array.append(Player(590, (400//2), x, self))
		
    def closeEvent(self, event):	
        #close Level	
        self.main_level.close() 
		
        for item in self.player_array:
            item.close()     		

        del self.main_level			
 




	
