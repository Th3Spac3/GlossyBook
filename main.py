import jieba
import sys

def parse(text):
    return " ".join(jieba.cut(text))

if __name__ == "__main__":
    
    if len(sys.argv) != 2:
        print("Usage: python main.py  <text>")
        sys.exit(1)

    text = sys.argv[1]
    f = open('message.txt', 'w', encoding='utf-8')
    f.write(parse(text))
    f.close()
      