# EcoSysteam

Izometric ecosystem PvP/PvE game being developed in Unity.
# Game description
As you start to play you can see a small ecosystem with players' creatures and the environment. The creatures are controlled by a primitive AI, whose goal is basic survival. Players can develop the AI's behavior and the characteristics of the creatures, thus occupying different economic niches. Various entities can be found in the environment, such as animals of different sizes, trees that grow plants around them, bushes that can be used for hiding. Players can acquire the points needed for development in various ways, all of which are related to survival in some way.
# Technologies used
* Unity game engine
* Unity Netcode for GameObjects
* Unity Transport

# Projektinformációk

Használt Unity verzió: 2022.3.10f1 (LTS)

Egy mappával beljebb található a projektmappa. Van benne egy unity-s gitignore, hogy ne commitoljunk feleslegesen olyan dolgokat, amiket generálni is lehet / platformfüggő.

A scriptek helye a scripts mappa, a prefabok helye a prefabs mappa.

Synchronizable a közös őse a ...Behaviour-oknak. Ebből származik a PlayerBehaviour.

A Synchronizable-leszármazottak ServerUpdate() függvénye felel a mozgás megvalósításáért, itt kell kiszámolni az új pozíciót.

A Unity-ben teszteléshez a HOST gombot kell megnyomni (miután elindult a játék): az szerver és kliens is egyben.

# Hálózati megoldások dokumentálása

## Felhasznált könyvtárak

A projekt Unity Netcode for GameObjects 1.6.0-t használ, alatta pedig Unity Transport 2.1.0 van. Legalább UTP 2.0.0 kell, hogy a webes verzió is működjön.

## Szinkronizáció

Alapvetően a szerver oldalon történik minden lényegi számítás, majd ezt a kliensoldal megkapja, megjelenítés céljából. A pozíció szinkronizációjához az NGO beépített NetworkTransform komponensét használjuk, ez minden mozgó entitáson rajta van. A skill tree szinkronizációját NetworkVariable-ök végzik, a PlayerSkillTree scriptben.

A játékos prefab tartalmazza az AI-t megvalósító PlayerBehaviour, illetve a PlayerSkillTree scriptet, és ezt a prefabot példányosítja a NetworkManager minden egyes kliens csatlakozásakor. Így lesz minden játékosnak hozzá tartozó skill tree-je. A fejlesztéseket kezelő GUI a klienshez tartozó player példány skill tree-jét módosítja, ami aztán a NetworkVariable segítségével megjelenik a szerveren is, és a player példányhoz tartozó PlayerBehaviour AI script a szerveren fel tudja használni a fejlesztett értékeket.

## Lobby

A játék indítása előtt egy lobby-ban lehet megvárni a játékosok csatlakozását, ekkor már felépül a kapcsolat. Majd a játék indításakor a NetworkManager SceneManager-e segítségével történik a játéknézet betöltése, minden kliensnél egyszerre, így kezdhető fair-en a játék.

# Felhasznált források:

Iain McManus: Needs-Based AI in unity


# Aktuális állapot

perpillanat annyi a demo "ai"-ja a dolognak, hogy:

- fa ill bokor nem csinál semmit (spawnolt helyen marad)
- kukac menekül, ha a player közel van
- Player kiválaszt egy random interactiont és azt megcsinálja, majd újat választ

