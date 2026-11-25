# Instrucciones para asistentes de IA

Este archivo lo leen Claude, Copilot, Cursor y otros agentes. Si sos humano y usás IA, pasale este contexto.

## Antes de escribir código
1. Leé [VISION.md](VISION.md): no implementes nada que la contradiga.
2. Seguí [docs/CONVENCIONES_CODIGO.md](docs/CONVENCIONES_CODIGO.md) al pie de la letra.
3. Trabajá sobre **un issue concreto**. Si no hay issue, pedilo o proponé uno.

## Reglas
- **Alcance:** cambiá solo lo que pide la tarea. Nada de refactors no pedidos, renombres masivos ni "mejoras" de paso.
- **Unity:** no edites a mano `.unity`, `.prefab`, `.asset` ni `.meta`. Si hace falta un cambio en escena o prefab, describilo como pasos para hacer en el Editor.
- **Datos:** todo valor de balance va en un ScriptableObject, nunca hardcodeado.
- **Paquetes:** no agregues dependencias.
- **Idioma:** código en inglés; comentarios, commits y PRs en español.
- **Commits:** Conventional Commits en español (ver [CONTRIBUTING.md](CONTRIBUTING.md)).
- **Dudas de diseño:** si algo de gameplay no está definido, no lo inventes: preguntá o dejalo explícito en el PR.

## Al terminar
- Explicá en el PR qué hiciste, qué probaste y qué tiene que verificarse en el Editor.
- Marcá en el PR que fue asistido por IA.
