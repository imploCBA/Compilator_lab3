# Compilator_lab3
### Вариант задания
Объявление словаря с инициализацией на языке Python

### Примеры допустимых строк
name = {'a':'f'};
name = {1:'f'};
name = {'a':'f', 'e':'w'};
name = {'a':'f', 6:'2qr'};

### Грамматика
1. ID -> letter IDREM
2. IDREM -> letter IDREM | digit IDREM | "=" OPEN
3. OPEN -> "{" KEY
4. KEY -> digit KEYDIGIT | "'" OPENKEYQUOT
5. OPENKEYQUOT -> letter KEYSTRING
6. KEYSTRING -> letter KEYSTRING | "'" CLOSEKEYQUOT
7. CLOSEKEYQUOT -> ":" COLON
8. KEYDIGIT -> digit KEYDIGIT | ":" COLON
9. COLON -> "'" OPENVALUEQUOT | digit VALUEDIGIT
10. OPENVALUEQUOT -> letter VALUESTRING
11. VALUESTRING -> letter VALUESTRING | "'" CLOSEVALUEQUOT
12. CLOSEVALUEQUOT -> "," KEY | "}" CLOSE
13. VALUEDIGIT -> "}" CLOSE
14. CLOSE -> ";" 


### Граф конечного автомата
![diagram (1)](https://github.com/imploCBA/Compilator_lab3/assets/60794005/19bdf180-ed70-4e64-bbe1-bd872a922c17)


### Тестовый пример
name = {'dss':'gas', 2:'fra', 'fd':3, 5:32};
