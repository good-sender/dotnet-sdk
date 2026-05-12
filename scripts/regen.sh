#!/usr/bin/env bash
# Regenerate this SDK from the bundled spec at openapi/goodsender.yaml.
#
# Usage:
#   scripts/regen.sh                  # standard regen, preserves LICENSE/README/CHANGELOG
#   scripts/regen.sh --clean          # wipe SDK output first (use after a library: change)
#
# Spec workflow: edit goodsender-web/openapi/goodsender/*.yaml in the spec source repo,
# bundle to a single YAML, copy that bundle into THIS repo's openapi/goodsender.yaml,
# then run this script. The script then runs openapi-generator-cli + the language-specific
# post-process embedded below.

set -euo pipefail

CLEAN=0
for arg in "$@"; do
  case "$arg" in
    --clean) CLEAN=1 ;;
    *) echo "!! unknown arg: $arg" >&2; exit 1 ;;
  esac
done

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SDK_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
SPEC="$SDK_DIR/openapi/goodsender.yaml"
CONFIG="$SCRIPT_DIR/generator-config.yaml"

# Generator JAR version. Override via OPENAPI_GENERATOR_VERSION env var.
# Wrapper version (the npm-published @openapitools/openapi-generator-cli that
# downloads the JAR). Override via OPENAPI_GENERATOR_CLI_VERSION env var.
CLI_WRAPPER_VERSION="${OPENAPI_GENERATOR_CLI_VERSION:-2.21.5}"
export OPENAPI_GENERATOR_VERSION="${OPENAPI_GENERATOR_VERSION:-7.21.0}"

[[ -f "$SPEC" ]] || { echo "!! spec not found at $SPEC" >&2; exit 1; }
[[ -f "$CONFIG" ]] || { echo "!! generator config not found at $CONFIG" >&2; exit 1; }

# --- .openapi-generator-ignore: protect hand-curated files from being overwritten
cat > "$SDK_DIR/.openapi-generator-ignore" <<'IGNORE_EOF'
LICENSE
CHANGELOG.md
README.md
tests/**
scripts/**
openapi/**
openapitools.json
api/openapi.yaml
IGNORE_EOF

# --- Optional clean step: wipe SDK output (preserves hand-curated files only)
if (( CLEAN == 1 )); then
  echo ">> --clean: wiping $SDK_DIR (preserving LICENSE, CHANGELOG.md, README.md, tests/, scripts/, openapi/, openapitools.json)"
  find "$SDK_DIR" -mindepth 1 -maxdepth 1 \
    ! -name LICENSE \
    ! -name CHANGELOG.md \
    ! -name README.md \
    ! -name tests \
    ! -name scripts \
    ! -name openapi \
    ! -name openapitools.json \
    ! -name .git \
    ! -name .gitignore \
    -exec rm -rf {} +
fi

# --- Regen via openapi-generator-cli
echo ">> regenerating csharp SDK"
(cd "$SDK_DIR" && npx --yes @openapitools/openapi-generator-cli@"$CLI_WRAPPER_VERSION" \
  generate \
  -i "$SPEC" \
  -c "$CONFIG" \
  -o "$SDK_DIR" \
  --skip-validate-spec)

# --- Language-specific post-process
# No post-process needed for .NET on the restsharp library.
:

echo ">> done."
