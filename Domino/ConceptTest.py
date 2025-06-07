# Define the pattern
_pattern = [
    ["     ", "     ", "     "],
    ["     ", "  o  ", "     "],
    [" o   ", "     ", "   o "],
    [" o   ", "  o  ", "   o "],
    [" o o ", "     ", " o o "],
    [" o o ", "  o  ", " o o "],
    [" o o ", " o o ", " o o "]
]

# Simulated ICard class
class Card:
    def __init__(self, left_face_value, right_face_value):
        self.left_face_value = left_face_value
        self.right_face_value = right_face_value

# Printing horizontally aligned tiles
def print_tile_horizontal(cards):
    for line in range(3):  # 3 lines per card pattern
        for i, card in enumerate(cards):
            print(" ", end="")  # spacing between cards

            print(
                f"{_pattern[card.left_face_value][line]}|{_pattern[card.right_face_value][line]}",
                end=" "
            )
        print()  # New line after all cards in one row

def print_tile_vertical(cards):
    for line in range(8):  # 3 lines top, 1 divider, 3 lines bottom, 1 line numbers
        for i, card in enumerate(cards):
            if line < 3:
                # Top half: left face
                print(" " + _pattern[card.left_face_value][line], end=" ")
            elif line == 3:
                # Divider line
                print(" -----", end=" ")
            elif line == 7:
                # Card index number centered under the tile
                print(f"   {i+1}  ", end=" ")
            else:
                # Bottom half: right face
                print(" " + _pattern[card.right_face_value][line - 4], end=" ")
        print()


# Example usage
cards = [Card(1, 3), Card(4, 5), Card(2, 6)]
print_tile_horizontal(cards)
print_tile_vertical(cards)