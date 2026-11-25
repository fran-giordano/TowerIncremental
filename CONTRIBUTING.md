# Cómo contribuir

Este proyecto nació como trabajo práctico de CG&RV (UPF, 2026/2) y ahora sigue como proyecto abierto, **sin fechas ni presión**. Se suma cualquier cosa: ideas, código, arte, música, sonido, diseño de niveles.

## Dónde está el roadmap
Todo vive en el tablero **GitHub Projects** del repo (pestaña *Projects*). Cada tarjeta es un *issue*.

| Columna | Significa |
|---|---|
| 💡 Ideas | Propuestas sin evaluar. Cualquiera puede agregar. |
| 📋 Para hacer | Idea aceptada y con alcance claro. |
| 🚧 En curso | Alguien la está haciendo (asignate el issue). |
| ✅ Hecho | Mergeado en `main`. |

Los **hitos** (*milestones*) agrupan trabajo por objetivo, no por fecha.

## Flujo
1. **Tenés una idea:** abrí un issue con la plantilla *💡 Idea*. Va solo a la columna Ideas.
2. **Querés hacer algo:** elegí un issue de *Para hacer*, asignátelo y movelo a *En curso*.
3. **Rama:** `tipo/descripcion-corta`, por ejemplo `feat/torre-laser`, `art/nuevo-castillo`, `audio/musica-menu`, `fix/gif-derrota`.
4. **Commits:** formato [Conventional Commits](https://www.conventionalcommits.org/): `feat:`, `fix:`, `art:`, `audio:`, `docs:`, `refactor:`.
5. **Pull Request** a `main` con `Closes #N` en la descripción. El issue se cierra solo al mergear.

## Etiquetas
- **Área:** `código`, `arte`, `audio`, `diseño de juego`, `UI/UX`, `docs`.
- **Tipo:** `idea`, `bug`, `mejora`.
- **Extras:** `para empezar` (tareas chicas e independientes), `necesita decisión`.

## Configurar Unity
- **Versión:** Unity **6000.3.28f1** (URP). Usá exactamente esa versión desde Unity Hub.
- **Archivos pesados:** evitá subir binarios de más de 50 MB. Si el repo empieza a pesar mucho, pasamos a Git LFS (está en el roadmap).
- **Merge de escenas y prefabs:** agregá esto a tu `.gitconfig` para que Git use la herramienta de Unity:
  ```ini
  [merge "unityyamlmerge"]
      name = Unity SmartMerge
      driver = '<ruta-de-Unity>/Editor/Data/Tools/UnityYAMLMerge' merge -p %O %B %A %A
  ```
- **Evitá conflictos:** dos personas no deberían editar la misma escena o el mismo prefab a la vez. Avisá en el issue qué escena o prefab vas a tocar.

## Assets de terceros
- Antes de agregar un asset, verificá su licencia y sumalo a la tabla de **Créditos** del README.
- Para lo nuevo, preferí licencias **CC0 o CC-BY** (por ejemplo Kenney.nl, OpenGameArt, Freesound).
- No subas música ni sonidos con copyright.
