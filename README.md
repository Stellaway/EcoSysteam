# EcoSysteam

Izometric ecosystem PvP/PvE game being developed in Unity.
# Game description
As you start to play you can see a small ecosystem with players' creatures and the environment. The creatures are controlled by a primitive AI, whose goal is basic survival. Players can develop the AI's behavior and the characteristics of the creatures, thus occupying different economic niches. Various entities can be found in the environment, such as animals of different sizes, trees that grow plants around them, bushes that can be used for hiding. Players can acquire the points needed for development in various ways, all of which are related to survival in some way.
# Technologies used
* Unity game engine
* Unity Netcode for GameObjects
* Unity Transport

# Információ kollaborátoroknak (by 1234ab)

Előre sry, egy kicsit katyvasz, de remélem vannak benne hasznos infók.

Használt Unity verzió: 2022.3.10f1 (LTS)

Lehet, hogy le kell tölteni 1-2 könyvtárat (pl netcode), ez nem tudom, mennyire automatikus, ki kell próbálni. Unity transport package-ből egy frissebbet szedtem le (2.1.0, 1.3.4 helyett, mert az kell elvileg a webes supporthoz), remélhetőleg nem romlik el emiatt.

Direkt van egy mappával beljebb a projektmappa, mint mobweben, így talán jobb. Tettem bele egy unity-s gitignore-t, hogy ne commitoljunk feleslegesen olyan dolgokat, amiket generálni is lehet / platformfüggő.

A scriptek helye a scripts mappa, a prefabok helye a prefabs mappa. Ezt javasolja a tutorial, mint unitys szokás.

Az elnevezéseket próbáljuk konzisztensen tartani snake_case/camelCase/PascalCase/ilyesmi, végülis ez egy fancy projekt amit a végén leadunk xd. Hasonlóan a kódstílust, mindegy milyen, de legyen minél konzisztensebb (pl {} pozíciója).

Synchronizable lett a közös ős neve végül, egyelőre. Ebből származik a PlayerBehaviour (valahogy ez a ...Behaviour a szokásos elnevezés, úgy tűnik). Próbáltam szépen kommentezni a kódot, ha valami nem világos, olvassátok el plíz :DD

Újragondolva, perpillanat nem látom értelmét a közös ősnek, mert van egy komponens ami tökéletesen megcsinálja a transzform szinkronizációt, interpolációval meg mindennel, szóval azt használjuk most. Ugyanakkor nem töröltem még ki a Synchronizable-t, hátha később lesz értelme egy közös ősnek. De perpillanat letörlésre van ítélve szerintem.

Amit majd ügyködni kell, az a PlayerBehaviour (illetve egyéb, AnimalBehaviour ill PlantBehaviour) ServerUpdate() függvénye. Itt kell kiszámolni az új pozíciót. Persze az még nem világos, hogy hogy fognak egymással kommunikálni az objektumok, illetve a spawnolás sem egyértelmű egyelőre.

Ha Unity-ben szeretnétek tesztelni, akkor a HOST gombot nyomjátok (miután elindítottátok a játékot): az szerver és kliens is egyben.

# Aktuális állapot

perpillanat annyi a demo "ai"-ja a dolognak, hogy:

- fa ill bokor nem csinál semmit (spawnolt helyen marad)

- kukac ill egér a spawnoláskor legközelebbi játékos irányába megy (más a sebességük)

- játékos jobbra megy folyamatosan, gomb megnyomásakor (már csak az a játékos, amelyik a gombját megnyomta) leidéz egy random dolgot, majd elteleportál egy random helyre

- JELENLEG NEM FOG ÍGY LEFUTNI, kell hozzáadni collidereket, de leginkább újragondolni az egészet...

