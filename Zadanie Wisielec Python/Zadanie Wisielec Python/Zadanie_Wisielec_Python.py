import random
import os
from enum import Enum

# ---- WordBank ----
class WordBank:
    _words = [
        "słońce","onomatopeja","auto","eufemizm","aluzja",
        "oksymoron","pleonazm","wiatrak","rower","kredka",
        "kot","pies","kurczak","lampa","źdźbło","czapka"
    ]

    @staticmethod
    def random_word():
        return random.choice(WordBank._words)


# ---- GameState ----
class GameState(Enum):
    PLAYING = 1
    WON     = 2
    LOST    = 3


# ---- Game ----
class Game:
    def __init__(self, secret_word):
        self.secret = secret_word
        self.correct = set()
        self.wrong = set()
        self.max_errors = 10

        # do rysowania ASCII‑art
        self.rows, self.cols = 7, 7
        self.canvas = [[' ']*self.cols for _ in range(self.rows)]
        self.draw_steps = [
            self._draw_main_post,
            self._draw_left_support,
            self._draw_right_support,
            self._draw_crane,
            self._draw_rope,
            self._draw_head,
            self._draw_body,
            self._draw_left_arm,
            self._draw_right_arm,
            self._draw_legs,
        ]

    @property
    def errors(self):
        return len(self.wrong)

    @property
    def state(self):
        if self.errors >= self.max_errors:
            return GameState.LOST
        if all(c in self.correct for c in self.secret):
            return GameState.WON
        return GameState.PLAYING

    def guess(self, ch):
        if self.state != GameState.PLAYING:
            return
        if ch in self.secret:
            self.correct.add(ch)
        else:
            self.wrong.add(ch)

    def get_masked_word(self):
        return ''.join(c if c in self.correct else '_' for c in self.secret)

    def wrong_guesses(self):
        return sorted(self.wrong)

    def draw(self):
        # wyczyść canvas
        for r in range(self.rows):
            for c in range(self.cols):
                self.canvas[r][c] = ' '
        # wykonaj kroki
        for i in range(min(self.errors, len(self.draw_steps))):
            self.draw_steps[i]()
        # renderuj
        for row in self.canvas:
            print(''.join(row))

    # --- poszczególne kroki rysowania ---
    def _draw_main_post(self):
        for r in range(6):
            self.canvas[r][1] = '│'

    def _draw_left_support(self):
        self.canvas[6][0] = '/'

    def _draw_right_support(self):
        self.canvas[6][2] = '\\'

    def _draw_crane(self):
        for c in range(1,5):
            self.canvas[0][c] = '─'
        self.canvas[0][1] = '┌'
        self.canvas[0][4] = '┐'

    def _draw_rope(self):
        self.canvas[1][4] = '│'

    def _draw_head(self):
        self.canvas[2][4] = '0'

    def _draw_body(self):
        self.canvas[3][4] = '│'

    def _draw_left_arm(self):
        self.canvas[3][3] = '/'

    def _draw_right_arm(self):
        self.canvas[3][5] = '\\'

    def _draw_legs(self):
        self.canvas[4][3] = '/'
        self.canvas[4][5] = '\\'


# ---- Main ----
def clear_screen():
    os.system('cls' if os.name=='nt' else 'clear')
    # print("\033[2J\033[H", end="")

def main():
    word = WordBank.random_word()
    game = Game(word)

    while game.state == GameState.PLAYING:
        clear_screen()
        print(f"Pozostało prób: {game.max_errors - game.errors}")
        print("Błędne litery:", ", ".join(game.wrong_guesses()))
        game.draw()
        print("Hasło: ", game.get_masked_word())
        guess = input("Podaj literę: ").strip().lower()

        if len(guess) == 1:
            game.guess(guess)
        else:
            # nieprawidłowe wejście liczy się jako błąd
            game.guess(None)

    # koniec gry
    clear_screen()
    game.draw()
    if game.state == GameState.WON:
        print("Gratulacje! Odgadłeś hasło:", word)
    else:
        print("Przegrałeś. Hasło to:", word)

if __name__ == "__main__":
    main()

