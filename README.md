# Założenia

Jest sporo mechanik i niewiele czasu, więc – jak przedstawiono w zadaniu – poszedłem w **prototyp**.  
Nie dodawałem więcej struktury do kodu niż to konieczne – żadnego `LevelController` itp.  
Na przykład dla skryptów UI wybrałem najprostszy sposób pozyskania odpowiednich referencji.  

Gdyby to był pełnoprawny projekt, system byłby bardziej rozbudowany i nie opierałby się na założeniach typu *"jest jeden gracz na levelu"*, czy hardcoded stringi.  
Do prototypu jednak uważam, że to dobre podejście.

---
# Czas

Założyłem, że zmieszczę się w ustalonym przez siebie limicie **8 godzin** – udało się, mimo że do dodania było sporo funkcjonalności.

---

# Napisanie logiki przenoszenia obiektów
Starałem się zachować 'symulatorowy' feeling, więc dałem możliwość rzutu obiektem przy upuszczaniu poprzez movement postaci lub szybki obrót kamery. 

---

# System wykrywania gracza przez kamery

Tutaj poszedłem w niebanalne podejście – tworzę **runtime mesh**, który wykrywa gracza.  
Wybrałem to rozwiązanie, bo zależało mi, aby w sensowny sposób pokazać graczowi obszar, w którym jest wykrywany.  

Myślę, że efekt jest całkiem fajny, choć oczywiście można było to zrobić dużo prościej – ale wtedy bez wizualizacji dla gracza.

---

# Reakcja na wykrycie

Na tym etapie brakowało już czasu, więc dodałem:

- zmiana materiału obszaru widzenia kamery 
- Detection progress bar  
- Bardzo prosty game over

---

# Refaktor

Dużo zmieniłem w dostarczonym kodzie, dostosowując go do swojego stylu programowania.  
Przy ewentualnej współpracy oczywiście dostosowuję się do tego, co aktualnie jest w codebase, ale tutaj był tego bardzo mały procent i miał się odbyć refactoring.

---

# Niedoskonałości

Na pewno znajdzie się trochę błędów i rzeczy do poprawy.  
Z tych, które przychodzą mi do głowy:

- Trzeba zablokować możliwość przenoszenia `TheftObject` poza obszar przez ściany.  
- UI jest w mocno debugowej wersji – priorytetem było zmieszczenie się w czasie.
