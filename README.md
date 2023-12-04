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

A projekt [Unity Netcode for GameObjects](https://docs-multiplayer.unity3d.com/netcode/current/installation/index.html) 1.6.0-t használ, alatta pedig Unity Transport 2.1.0 van. Legalább UTP 2.0.0 kell, hogy a webes verzió is működjön.

## Szinkronizáció

Alapvetően a szerver oldalon történik minden lényegi számítás, majd ezt a kliensoldal megkapja, megjelenítés céljából. A pozíció szinkronizációjához az NGO beépített NetworkTransform komponensét használjuk, ez minden mozgó entitáson rajta van. A skill tree szinkronizációját NetworkVariable-ök végzik, a PlayerSkillTree scriptben.

A játékos prefab tartalmazza az AI-t megvalósító PlayerBehaviour, illetve a PlayerSkillTree scriptet, és ezt a prefabot példányosítja a NetworkManager minden egyes kliens csatlakozásakor. Így lesz minden játékosnak hozzá tartozó skill tree-je. A fejlesztéseket kezelő GUI a klienshez tartozó player példány skill tree-jét módosítja, ami aztán a NetworkVariable segítségével megjelenik a szerveren is, és a player példányhoz tartozó PlayerBehaviour AI script a szerveren fel tudja használni a fejlesztett értékeket.

## Lobby

A játék indítása előtt egy lobby-ban lehet megvárni a játékosok csatlakozását, ekkor már felépül a kapcsolat. Majd a játék indításakor a NetworkManager SceneManager-e segítségével történik a játéknézet betöltése, minden kliensnél egyszerre, így kezdhető fair-en a játék.

# AI

A játék mesterséges intelligenciája szükséglet alapú.
A játék szereplői mind intelligens objektumok (SmartObject), amik képesek hirdetni különböző akciókat (Interactions)
és ezeknek az akcióknak a hatásait (StatChanges).
Az AI, ahelyett, hogy megállapítaná az egyes tárgyakról, hogy milyen értékesek számára, a tárgyak által kínált hirdetéseket veszi figyelembe.
Egy intelligens objektum több interakciót is hirdethet, a játékos ezek közül választ egyet.

Az AI nem lát minden objektumot, van egy bizonyos érzékelési határa.
Arról, hogy elérhetőek legyenek számára az intelligens objektumok és ezáltal az interakciók, egy SmartObjectManager gondoskodik.

Az AI döntéskor figyelembe veszi az elérhető interakciók pontszámát.
A pont számítás a ScoreInteractions függvényben történik. 
Az AI figyelembe veszi: az interakció típusát, változás mértékét, a veszélyt és az objektum távolságát.

Az AI reagál a PlayerSkillTreeben történő fejlesztésekre, a gondolkodása az egyes attribútumszorzókban valósul meg.
Pl.: ha a játékos kifejleszti a húsevőséget, akkor hirtelen a hús-t adó interakció kedvezőbb lesz neki.

A játékos mohó és hóbortos. Ha talál egy célt, akkor azt energiát nem kímélve üldözi, azonban ha nincs semmi interakció, ami felkeltette az érdeklődését, bolyong, tétlen.

## Felhasznált források:

Iain McManus: Needs-Based AI in unity

# Balance

A balanszolás kényes folyamat. A sokszínű interakciók bonyolult módon hatnak egymásra.
A legnagyobb kihívás a játékos éhségének és a pontszerzés sebességének meghatározása volt.
Sok iterációban lehetetlen volt életben maradni, míg fejlődött a kis alienünk, sok iterációban lehetetlen volt meghalni.
A mostani verzió hagy még némi kevetnivalót, azonban egész jó játékélményt biztosít.

Jelenleg is fennálló balanszolási probléma, hogy a húsevés kifejlesztése nagyon erős, azonban így is figyelnünk kell a játékos környezetére, mivel könnyedén csapdába ejthet a kis űrlény mohó gondolkodása.

Minden meglátást szívesen fogadunk a playtestereinktől. :))

## Playtest meglátások, jövőbeli tervek
    - Jó lenne, ha a játékosok ennék egymást
    - A fák előbb-utóbb elsorvadhatnának
    - AI viewdistance-hez egy kör vagy valami
