# Cómo trabajamos

Somos pocos, sin fechas y cada uno avanza cuando puede. Estas reglas existen para que el otro entienda qué pasó sin tener que preguntar.

## 1. Dónde vive cada cosa
| Qué | Dónde |
|---|---|
| Charla rápida | WhatsApp / Discord. Si sale algo importante, se pasa a GitHub. |
| Qué juego hacemos | [VISION.md](VISION.md) |
| Ideas y tareas | Issues de GitHub |
| En qué está cada uno | Tablero (pestaña *Projects*) |
| Reglas de código | [docs/CONVENCIONES_CODIGO.md](docs/CONVENCIONES_CODIGO.md) |

## 2. De idea a tarea
```
💡 Idea ──► se comenta ──► Francisco decide ──► 📋 Tarea ──► 🚧 Haciendo ──► ✅ Hecho
                                  └─► se cierra (queda registrada como descartada)
```
1. **Idea:** cualquiera abre un issue con la plantilla *💡 Idea*. Puede ser de mecánica, arte, sonido, cualquier cosa.
2. **Se comenta:** el otro opina en el issue (👍 / 👎 o un comentario).
3. **Se decide:** Francisco tiene la última palabra.
   - **Aprobada:** se crean una o más *📋 Tareas* que la referencian.
   - **Descartada:** se cierra como *Not planned*. No se borra: así no se vuelve a discutir.
4. Si la idea cambia la visión, se actualiza [VISION.md](VISION.md) en el mismo PR o en uno aparte.

### Regla para no frenarse
Si algo **no cambia la visión** (un bug, un ajuste de balance, refactor, una mejora chica), **se hace directo**, sin abrir una idea. Igual va por PR.

## 3. Tomar una tarea
1. Asignátela (*Assignees* → vos) y movela a **🚧 Haciendo**.
2. Si vas a tocar una **escena** o un **prefab compartido**, aclaralo en el issue: Unity no mergea bien dos cambios en la misma escena.
3. Si la abandonás, desasignate y dejá un comentario de dónde quedó.

## 4. Ramas
`tipo/numero-descripcion-corta`, en minúsculas y con guiones:
```
feat/12-olas-infinitas
fix/18-torre-no-dispara
art/7-modelo-castillo
docs/3-vision-tematica
```
Nunca se trabaja directo en `main`.

## 5. Commits
Formato [Conventional Commits](https://www.conventionalcommits.org/es/), **en español y en imperativo**:
```
feat: agregar olas infinitas con escalado de vida
fix: corregir torre que no detectaba enemigos lentos
refactor: extraer cálculo de daño a DamageCalculator
```
| Tipo | Uso |
|---|---|
| `feat` | Funcionalidad nueva |
| `fix` | Corrección de bug |
| `refactor` | Cambio de código sin cambiar comportamiento |
| `art` | Modelos, materiales, VFX, escenario |
| `audio` | Música y sonidos |
| `balance` | Solo cambios de valores (ScriptableObjects) |
| `docs` | Documentación |
| `chore` | Configuración, paquetes, limpieza |

Un commit = un cambio lógico. Nada de "cambios varios".

## 6. Pull Requests
- Uno por tarea, chico (idealmente menos de unos 400 líneas de código).
- En la descripción va `Closes #N`, para que el issue se cierre solo.
- Completá el checklist de la plantilla.
- **Revisión:** si el otro está disponible, revisa. Si en **48 h** nadie revisó y el PR no toca la visión, el autor puede mergear.
- Se mergea con **Squash and merge** (un commit limpio por PR en `main`).

## 7. Definición de "hecho"
- [ ] Funciona en Play Mode sin errores en la consola.
- [ ] Sigue las [convenciones de código](docs/CONVENCIONES_CODIGO.md).
- [ ] Los valores de juego están en ScriptableObjects, no hardcodeados.
- [ ] Si agrega un asset de terceros, figura en *Créditos* con su licencia.
- [ ] Si cambia algo de la visión, VISION.md está actualizado.

## 8. Etiquetas
| Etiqueta | Significa |
|---|---|
| `idea` | Propuesta sin decidir |
| `tarea` | Trabajo aprobado y concreto |
| `bug` | Algo no funciona |

## 9. Assets y licencias
- Solo assets **CC0 o CC-BY** (Kenney.nl, Quaternius, OpenGameArt, Freesound) o propios.
- Nada con copyright: ni música comercial, ni sonidos sacados de otros juegos.
- Cada asset externo se anota en la sección *Créditos* de [docs/CREDITOS.md](docs/CREDITOS.md).
