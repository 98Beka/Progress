## Проект по созданию игры "Морской бой" на ASP.Net Core от команды Progress
### Правила игры

#### Правила размещения кораблей (флота)

Игровое поле — квадрат 10×10 у каждого игрока, на котором размещается флот кораблей. 
Горизонтали нумеруются сверху вниз, а вертикали помечаются буквами слева направо.
При этом используются буквы русского алфавита от «а» до «к» (буквы «ё» и «й» пропускаются).

Размещаются:
* 1 корабль — ряд из 4 клеток («четырёхпалубный»; линкор)
* 2 корабля — ряд из 3 клеток («трёхпалубные»; крейсера)
* 3 корабля — ряд из 2 клеток («двухпалубные»; эсминцы)
* 4 корабля — 1 клетка («однопалубные»; торпедные катера)

При размещении корабли не могут касаться друг друга сторонами и углами.

Рядом со «своим» полем размещается «чужое» такого же размера, только пустое. Это участок моря, где плавают корабли противника.

При попадании в корабль противника — на чужом поле ставится крестик, при холостом выстреле — точка. Попавший стреляет ещё раз.

Самыми уязвимыми являются линкор и торпедный катер: первый из-за крупных размеров, в связи с чем его сравнительно легко найти, а второй из-за того, что топится с одного удара, хотя его найти достаточно сложно.

#### Потопление кораблей противника
1. Ожидающий подключения игрок ходит первым.

2. Игрок, выполняющий ход, совершает выстрел — нажимает на поле противника на клетку, в которой, по его мнению, находится корабль противника, например, «В1».

3. Если выстрел пришёлся в клетку, не занятую ни одним кораблём противника, то следует сообщение «Мимо!» и на чужом квадрате в этом месте появляется точка. Право хода переходит к сопернику.
Если выстрел пришёлся в клетку, где находится многопалубный корабль (размером больше чем 1 клетка), то следует сообщение «Ранил!» или «Попал!», кроме одного случая (см. далее).  В этом месте на чужом поле появляется крестик, а у противника появляется крестик на своём поле также в эту клетку. Стрелявший игрок получает право на ещё один выстрел.
Если выстрел пришёлся в клетку, где находится катер, или последнюю непоражённую клетку многопалубного корабля, то следует ответ «Убил!» или «Потопил!». У двоих игроков отмечается потопленный корабль на соответтствующем поле. Стрелявший игрок получает право на ещё один выстрел.

4. Победителем считается тот, кто первым потопит все 10 кораблей противника. 