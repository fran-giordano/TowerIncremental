# Convenciones de código y proyecto

Estas reglas valen igual para código escrito a mano o con IA. Si algo no está cubierto, se sigue el estilo del código que ya existe alrededor.

## 1. Idioma
| Qué | Idioma |
|---|---|
| Código (clases, métodos, variables, carpetas de scripts) | **Inglés** |
| Comentarios, commits, issues, PRs, docs | **Español** |
| Textos que ve el jugador | Español, siempre en un solo lugar (nunca hardcodeados en scripts) |

## 2. Estructura de carpetas
```
Assets/
├─ _Project/                 ← todo lo nuestro
│  ├─ Scripts/
│  │  ├─ Core/               GameManager, estados, eventos, guardado, FX
│  │  ├─ Towers/             Torres, proyectiles, daño, colocación
│  │  ├─ Enemies/            Enemigos, oleadas, caminos
│  │  ├─ Meta/               Taller, gemas, progreso permanente
│  │  ├─ UI/
│  │  ├─ Art/                Animaciones y efectos visuales por código
│  │  └─ Utils/              Pools, formateo de números, extensiones
│  ├─ Data/                  ScriptableObjects (.asset) por tipo
│  ├─ Prefabs/
│  ├─ Scenes/
│  └─ Art/                   Modelos, materiales, texturas, audio, VFX
├─ ThirdParty/               ← assets externos, sin modificar
├─ Settings/                 ← ajustes de URP (no mover)
└─ TextMesh Pro/             ← lo maneja Unity (no mover)
```
- No se edita nada dentro de `ThirdParty/`. Para cambiar un asset externo, se duplica en `_Project/`.
- Las carpetas `Meta/` y `Utils/` se crean cuando haga falta.
- Namespace = `TowerIncremental.` + carpeta de primer nivel dentro de `Scripts/` (ej.: `TowerIncremental.Towers`).

## 3. Nombres (C#)
| Elemento | Estilo | Ejemplo |
|---|---|---|
| Clases, structs, enums, interfaces | `PascalCase` (interfaces con `I`) | `TowerData`, `IDamageable` |
| Métodos y propiedades | `PascalCase` | `ApplyDamage()`, `MaxHealth` |
| Campos privados y `[SerializeField]` | `camelCase`, sin prefijo | `fireInterval` |
| Parámetros y variables locales | `camelCase` | `targetPosition` |
| Constantes y `static readonly` | `PascalCase` | `MaxTowersPerSlot` |
| Eventos | `On` + verbo en pasado | `OnEnemyKilled` |
| Booleanos | prefijo `is`/`has`/`can` | `isAlive`, `canUpgrade` |

- Un archivo por clase, con el mismo nombre que la clase.
- Namespace por carpeta: `TowerIncremental.Towers`, `TowerIncremental.Meta`…
- Modificador de acceso explícito solo cuando no es `private` (el `private` se omite).

## 4. Reglas de Unity
1. **Datos en ScriptableObjects.** Stats, costos, oleadas y curvas de escalado van en `*Data` (`TowerData`, `EnemyData`, `WaveConfig`…). Nada de números mágicos en scripts.
2. **Campos del Inspector:** `[SerializeField]` sobre un campo privado, nunca `public` solo para verlo en el Inspector. Hacia afuera se expone con una propiedad de solo lectura.
3. **Nada costoso en `Update`:** ni `GetComponent`, ni `Find*`, ni `new` de colecciones. Se cachea en `Awake`.
4. **Referencias:** se asignan en el Inspector o se inyectan. `FindFirstObjectByType` solo como último recurso y únicamente en `Awake`.
5. **Comunicación entre sistemas por eventos** (C# `event Action<T>`) y no con referencias cruzadas. Ejemplo: `Enemy` dispara `OnKilled` y la economía escucha. Suscribirse en `OnEnable` y desuscribirse en `OnDisable`.
6. **Pooling obligatorio** para lo que se crea muchas veces (enemigos, proyectiles, VFX, números de daño). Se usa `UnityEngine.Pool.ObjectPool<T>`.
7. **Números grandes:** los valores que escalan sin límite (oro, gemas, vida y daño en olas altas) usan `double`. En pantalla siempre se muestran con el formateador común (`1.2K`, `3.4M`…).
8. **Guardado:** un único `SaveSystem` que serializa a JSON en `Application.persistentDataPath`. Ningún otro script escribe archivos ni `PlayerPrefs`.
9. **Escenas:** una persona por escena a la vez (se avisa en el issue). Siempre que se pueda, el trabajo se hace en prefabs, no directo en la escena.
10. **Archivos `.meta`:** se commitean siempre junto a su archivo y no se editan a mano.

## 5. Estilo
- Indentación de 4 espacios y llaves en línea nueva (estilo Allman), como define `.editorconfig`.
- Métodos cortos, de una responsabilidad. Si pasa de unas 40 líneas, se divide.
- Los comentarios explican el **por qué**, no el qué.
- Sin código comentado "por si acaso": para eso está git.
- `MonoBehaviour` delgados: la lógica pura (cálculos de daño, escalado, economía) va en clases C# normales, que son fáciles de testear.

## 6. Paquetes
Ningún paquete nuevo (Package Manager o Asset Store) sin una tarea aprobada que lo justifique.
