
#import resources.font_resources
import sys
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5 import QtGui, QtCore

from source.score_window import *
from source.main_window import *
from source.game_window import *


class Berzerk(QMainWindow):

    num_players = 0

    def __init__(self):
        super().__init__()
		
        self.init_ui()

    def init_ui(self):
	
        QFontDatabase.addApplicationFont('resources/Glass_TTY_VT220.ttf')	

        #set initial game argument
        self.script = 'scr_a0000'
        self.game_engine_id = 0
        self.status = 'main'

        self.setWindowTitle('Berzerk')
        self.setFixedSize(640, 480)

        #set multiple stack layout
        self.stacked_layout = QStackedLayout()
		
		
        #create main window layout
        self.main_window = MainWindow()
        self.main_window.create_main_window_layout()
        self.stacked_layout.addWidget(self.main_window.main_window_widget)

        #set game window
        self.game_window = GameWindow()
        self.stacked_layout.addWidget(self.game_window)

        # set score window
        self.score_window = ScoreWindow()
        self.stacked_layout.addWidget(self.score_window)
		
        #set the central widget to display the layout
        self.central_widget = QWidget()
        self.central_widget.setLayout(self.stacked_layout)
        self.setCentralWidget(self.central_widget)

        #main window connection
        self.main_window.main_play_button.clicked.connect(self.start)
        self.main_window.main_multiplay_button.clicked.connect(self.multiplay)
        self.main_window.main_score_button.clicked.connect(self.score)
        self.main_window.main_quit_button.clicked.connect(self.quit)
		
        self.main_window.main_multiplay2_button.clicked.connect(self.multiplaylocal)	
        self.main_window.main_multiplaynetwork_button.clicked.connect(self.multiplaynetwork)			

        self.keys_pressed = set()		
	
        # use a timer to get 60Hz refresh (hopefully)
        self.timer = QBasicTimer()
        self.timer.start(16, self)
        self.max = 0
		
    def timerEvent(self, event):
        player_speed =  1
		
		
        if self.status == 'game_window':
            player_deaths = [a for a in self.game_window.player_array if a.death == False]
        
            if len(player_deaths) == 0:
                self.game_window.level_clear()
                self.game_window.level_new(self.num_players)  
                self.timer.start(16, self)				

        if self.status == 'game_window':
            self.kraj = False
            for j in range(self.num_players):
                if self.game_window.player_array[j].player_life == 0:
                    if self.game_window.player_array[j].bullet != None:
                        self.game_window.player_array[j].bullet.close()
                        self.game_window.player_array[j].bullet = None

                    self.game_window.player_array[j].close()
                    del self.game_window.player_array[j]
                    self.num_players -= 1
                    self.timer.start(16, self)
                    break
                

            if self.num_players > 1:
                for y in range(self.num_players):
                    if self.game_window.player_array[y].player_score > self.max:
                        self.max = self.game_window.player_array[y].player_score

            elif self.num_players == 1:
                if self.game_window.player_array[0].player_score > self.max:
                    self.max = self.game_window.player_array[0].player_score


            elif self.num_players == 0:
                self.game_window.close()
                self.timer.start(16, self)
                self.score()
                self.kraj = True

            if self.kraj == True:
                self.score_window._newScore("Player", self.max)

       
        if self.status == 'score_window' and Qt.Key_Escape in self.keys_pressed :
            self.keys_pressed.remove(Qt.Key_Escape)
            self.score_window.close()
            self._go_to_main()

        if self.status == 'mainmultyplay' and Qt.Key_Escape in self.keys_pressed:
            self.keys_pressed.remove(Qt.Key_Escape)
            self._exit_multiplay_menu()

        if self.status == 'main' and Qt.Key_Escape in self.keys_pressed:
            self.close()

        elif self.status == 'game_window' and Qt.Key_Escape in self.keys_pressed:	
            self.keys_pressed.remove(Qt.Key_Escape)			
            self.game_window.close()
            self.max = 0
            self._go_to_main()	

        else:
            #PLAYER 1
            if self.status == 'game_window' and Qt.Key_Up in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._move(0, -1 * player_speed,"U")
                self.game_window.player_array[0].last_key = 0				
            if self.status == 'game_window' and Qt.Key_Down in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._move(0, 1 * player_speed,"D")	
                self.game_window.player_array[0].last_key = 2	
            if self.status == 'game_window' and Qt.Key_Left in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._move(-1 * player_speed, 0,"L")
                self.game_window.player_array[0].last_key = 3					
            if self.status == 'game_window' and Qt.Key_Right in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._move(1 * player_speed, 0,"R")	
                self.game_window.player_array[0].last_key = 1	
            if self.status == 'game_window' and Qt.Key_Up not in self.keys_pressed and Qt.Key_Down not in self.keys_pressed and Qt.Key_Left not in self.keys_pressed and Qt.Key_Right not in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._move(0, 0,"-")
            if self.status == 'game_window' and Qt.Key_Enter in self.keys_pressed and self.game_window.player_array[0].player_ind == "player1":
                self.game_window.player_array[0]._shoot(self.game_window.main_level)

            #PLAYER 2
            if self.num_players > 1 and self.game_window.player_array[0].player_ind == "player1":
                if self.status == 'game_window' and Qt.Key_W in self.keys_pressed:
                    self.game_window.player_array[1]._move(0, -1 * player_speed,"U")
                    self.game_window.player_array[1].last_key = 0
                if self.status == 'game_window' and Qt.Key_S in self.keys_pressed:
                    self.game_window.player_array[1]._move(0, 1 * player_speed,"D")
                    self.game_window.player_array[1].last_key = 2
                if self.status == 'game_window' and Qt.Key_A in self.keys_pressed:
                    self.game_window.player_array[1]._move(-1 * player_speed, 0,"L")
                    self.game_window.player_array[1].last_key = 3
                if self.status == 'game_window' and Qt.Key_D in self.keys_pressed:
                    self.game_window.player_array[1]._move(1 * player_speed, 0,"R")
                    self.game_window.player_array[1].last_key = 1
                if self.status == 'game_window' and Qt.Key_W not in self.keys_pressed and Qt.Key_S not in self.keys_pressed and Qt.Key_D not in self.keys_pressed and Qt.Key_A not in self.keys_pressed:
                    self.game_window.player_array[1]._move(0, 0,"-")
                if self.status == 'game_window' and Qt.Key_Space in self.keys_pressed:
                    self.game_window.player_array[1]._shoot(self.game_window.main_level)

            elif self.num_players == 1 and self.game_window.player_array[0].player_ind == "player2":
                if self.status == 'game_window' and Qt.Key_W in self.keys_pressed:
                    self.game_window.player_array[0]._move(0, -1 * player_speed,"U")
                    self.game_window.player_array[0].last_key = 0
                if self.status == 'game_window' and Qt.Key_S in self.keys_pressed:
                    self.game_window.player_array[0]._move(0, 1 * player_speed,"D")
                    self.game_window.player_array[0].last_key = 2
                if self.status == 'game_window' and Qt.Key_A in self.keys_pressed:
                    self.game_window.player_array[0]._move(-1 * player_speed, 0,"L")
                    self.game_window.player_array[0].last_key = 3
                if self.status == 'game_window' and Qt.Key_D in self.keys_pressed:
                    self.game_window.player_array[0]._move(1 * player_speed, 0,"R")
                    self.game_window.player_array[0].last_key = 1
                if self.status == 'game_window' and Qt.Key_W not in self.keys_pressed and Qt.Key_S not in self.keys_pressed and Qt.Key_D not in self.keys_pressed and Qt.Key_A not in self.keys_pressed:
                    self.game_window.player_array[0]._move(0, 0,"-")
                if self.status == 'game_window' and Qt.Key_Space in self.keys_pressed:
                    self.game_window.player_array[0]._shoot(self.game_window.main_level)


            for x in range(self.num_players):

                #Player collide
                if self.status == 'game_window':

                   if self.game_window.player_array[x].death == True:# and self.game_window.player_array[0].animacija._current_frame == 13:
                      
                      if self.game_window.player_array[x].isVisible():
                          print('visible')
                          self.timer.start(16, self)
                          self.game_window.player_array[x].hide()
                      #self.game_window.player_array[x]._live()
                      #self.game_window.level_clear()
                      #self.game_window.level_new(self.num_players)

                   elif self.game_window.player_array[x].new_level_ind == True:
                       self.timer.start(16, self)
                       self.game_window.player_array[x].new_level_ind = False
                       self.game_window.level_clear()
                       self.game_window.level_new(self.num_players)

                   elif self.game_window.player_array[x].death == False and self.game_window.player_array[x]._collideEnemy(self.game_window.main_level.enemy_array):
                      print('collide enemy')
                      self.game_window.player_array[x]._dead()
                      if self.game_window.player_array[x].player_ind == "player1":
                          self.game_window.player_life_label.setText(
                              "Life: {0}".format(self.game_window.player_array[x].player_life))
                      elif self.game_window.player_array[x].player_ind == "player2":
                          self.game_window.player_life_label2.setText(
                              "Life: {0}".format(self.game_window.player_array[x].player_life))
                      self.timer.stop()
                      self.timer.start(1500, self)

                   elif self.game_window.player_array[x].death == False and self.game_window.player_array[x]._collide(self.game_window.main_level.wall_array):
                      print('collide walls')
                      self.game_window.player_array[x]._dead()
                      if self.game_window.player_array[x].player_ind == "player1":
                          self.game_window.player_life_label.setText(
                              "Life: {0}".format(self.game_window.player_array[x].player_life))
                      elif self.game_window.player_array[x].player_ind == "player2":
                          self.game_window.player_life_label2.setText(
                              "Life: {0}".format(self.game_window.player_array[x].player_life))
                      self.timer.stop()
                      self.timer.start(1500, self)

                   elif self.game_window.player_array[x].death == False and self.game_window.player_array[x]._collideDoor(self.game_window.main_level.door_array):
                      print('collide door')
                      self.game_window.player_array[x]._newLevel()
                      self.timer.stop()
                      self.timer.start(1500, self)

                   elif self.game_window.player_array[x].death == False and self.game_window.main_level.otto_exists == True:
                       if self.game_window.player_array[x]._collideOtto(self.game_window.main_level.otto):
                           print('collide otto')
                           self.game_window.player_array[x]._dead()
                           if self.game_window.player_array[x].player_ind == "player1":
                               self.game_window.player_life_label.setText(
                                   "Life: {0}".format(self.game_window.player_array[x].player_life))
                           elif self.game_window.player_array[x].player_ind == "player2":
                               self.game_window.player_life_label2.setText(
                                   "Life: {0}".format(self.game_window.player_array[x].player_life))
                           self.timer.stop()
                           self.timer.start(1500, self)

                   if self.game_window.player_array[x].death == False and self.game_window.main_level.deus_ex_machina_exists == True:

                        if self.game_window.player_array[x]._collideDeus_ex_machina(self.game_window.main_level.deus_ex_machina):
                            print('collide deus_ex_machina')
                            if self.game_window.player_array[x].player_ind == "player1":
                                self.game_window.player_life_label.setText(
                                    "Life: {0}".format(self.game_window.player_array[x].player_life))
                            elif self.game_window.player_array[x].player_ind == "player2":
                                self.game_window.player_life_label2.setText(
                                    "Life: {0}".format(self.game_window.player_array[x].player_life))


                #Bullets
                if self.status == 'game_window':
                    if self.game_window.player_array[x].bullet != None:
                       self.game_window.player_array[x].bullet._move(self.game_window.player_array[x].last_key)

                       if self.game_window.player_array[x].bullet._collideWall(self.game_window.main_level.wall_array):
                            print('bullet collide wall')
                            self.game_window.player_array[x].bullet.close()
                            self.game_window.player_array[x].bullet = None

                       elif self.game_window.player_array[x].bullet._collideEnemy(self.game_window.main_level.enemy_array):
                            print('bullet collide enemy')
                            self.game_window.player_array[x].player_score += 50
                            if self.game_window.player_array[x].player_ind == "player1":
                                self.game_window.player_score_label.setText("{0}".format(self.game_window.player_array[x].player_score)	)
                            elif self.game_window.player_array[x].player_ind == "player2":
                                self.game_window.player_score_label2.setText("{0}".format(self.game_window.player_array[x].player_score))
                            self.game_window.player_array[x].bullet.close()
                            self.game_window.player_array[x].bullet = None

                       elif self.game_window.player_array[x].bullet._collideWall(self.game_window.main_level.door_array):
                            print('bullet collide door')
                            self.game_window.player_array[x].bullet.close()
                            self.game_window.player_array[x].bullet = None

            # Enemy move
            if self.status == 'game_window':
                self.game_window.main_level.enemy_move(self.game_window.player_array)
                self.game_window.main_level.enemy_shoot()
                if self.game_window.main_level.enemy_bullet_collide(self.game_window.player_array) == True:


                    if self.game_window.player_array[0].player_ind == "player1":
                        self.game_window.player_life_label.setText(
                            "Life: {0}".format(self.game_window.player_array[0].player_life))

                    if self.game_window.player_array[0].player_ind == "player2":
                        self.game_window.player_life_label2.setText(
                            "Life: {0}".format(self.game_window.player_array[0].player_life))

                    elif self.num_players > 1 and self.game_window.player_array[1].player_ind == "player2":
                        self.game_window.player_life_label2.setText(
                            "Life: {0}".format(self.game_window.player_array[1].player_life))

                    self.timer.stop()
                    self.timer.start(1500, self)


                self.game_window.main_level.otto_create(self.game_window.player_array)

                if self.game_window.main_level.otto_exists == True:
                    self.game_window.main_level.otto_move(self.game_window.player_array)

            # DEUS_EX_MACHINA
            if self.status == 'game_window':
                self.game_window.main_level._create_deus_ex_machina()


    def keyPressEvent(self, event):
        self.keys_pressed.add(event.key())	

    def score(self):

        print('score')

        self.status = 'score_window'
        self.score_window._showScore()
        self.stacked_layout.setCurrentWidget(self.score_window)
		
    def keyReleaseEvent(self, event):
        if 	event.key() in self.keys_pressed:	
           self.keys_pressed.remove(event.key())	
	
 	
    def start(self):

        print('start')

        self.status = 'game_window'

        self.num_players = 1
        self.game_window.game_new(self.num_players);
        self.stacked_layout.setCurrentWidget(self.game_window)

    def start2players(self):

        print('start2players')

        self.status = 'game_window'

        self.num_players = 2
        self.game_window.game_new(self.num_players);
        self.stacked_layout.setCurrentWidget(self.game_window)

    def multiplay(self):

        self.status = 'mainmultyplay'

        self.main_window.main_multiplay2_button.show()
        self.main_window.main_multiplaynetwork_button.show()		
        self.main_window.main_multiplay2_button.setEnabled(True)	
        self.main_window.main_multiplaynetwork_button.setEnabled(True)		
	
        self.main_window.main_play_button.setEnabled(False)
        self.main_window.main_multiplay_button.setEnabled(False)	
        self.main_window.main_score_button.setEnabled(False)		
        self.main_window.main_quit_button.setEnabled(False)		
		
			
        print('multiplay')		
		
    def multiplaylocal(self):

        print('multiplaylocal')

        self.start2players()

        #self.main_window.main_multiplay2_button.hide()
        #self.main_window.main_multiplaynetwork_button.hide()
		

        #self.main_window.main_play_button.setEnabled(True)
        #self.main_window.main_multiplay_button.setEnabled(True)
        #self.main_window.main_score_button.setEnabled(True)
        #self.main_window.main_quit_button.setEnabled(True)

    def multiplaynetwork(self):

        print('multiplaynetwork')
		
        self.main_window.main_multiplay2_button.hide()
        self.main_window.main_multiplaynetwork_button.hide()		
		
        self.main_window.main_play_button.setEnabled(True)
        self.main_window.main_multiplay_button.setEnabled(True)	
        self.main_window.main_score_button.setEnabled(True)		
        self.main_window.main_quit_button.setEnabled(True)		

    def quit(self):

        print('quit')
        self.close()

    def _go_to_main(self):


        self.status = 'main'
        self.stacked_layout.setCurrentWidget(self.main_window.main_window_widget)


        self.main_window.main_multiplay2_button.hide()
        self.main_window.main_multiplaynetwork_button.hide()

        self.main_window.main_play_button.setEnabled(True)
        self.main_window.main_multiplay_button.setEnabled(True)
        self.main_window.main_score_button.setEnabled(True)
        self.main_window.main_quit_button.setEnabled(True)

    def _exit_multiplay_menu(self):
        self.status = 'main'

        self.main_window.main_multiplay2_button.hide()
        self.main_window.main_multiplaynetwork_button.hide()
        self.main_window.main_play_button.setEnabled(True)
        self.main_window.main_multiplay_button.setEnabled(True)
        self.main_window.main_score_button.setEnabled(True)
        self.main_window.main_quit_button.setEnabled(True)

    def closeEvent(self, event):

        print('close')
