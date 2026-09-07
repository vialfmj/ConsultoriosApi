# Instrucciones del proyecto

Respondé siempre en español (Argentina), incluyendo comentarios de código, commits y explicaciones.

## Agregar funcionalidad nueva

Antes de explorar el código fuente para agregar una funcionalidad nueva (endpoint, caso de uso, entidad, repositorio), consultá primero los patrones ya documentados en `.claude/agents/dominio-layer.md`, `application-layer.md`, `persistence-layer.md`, `api-layer.md` y `test-layer.md` — ya resumen el patrón CQRS del proyecto con ejemplos reales por capa, así no hace falta re-descubrirlo leyendo el código cada vez.

Esos mismos archivos sirven para delegar la implementación en el subagente correspondiente (vía `Agent`) cuando el cambio es grande o conviene paralelizar por capa. Para cambios chicos, alcanza con leerlos como referencia e implementar directo.

Al terminar un cambio, seguí lo que indica `test-layer.md`: correr los tests y, si corresponde, actualizar el `.http` de la capa Api.
