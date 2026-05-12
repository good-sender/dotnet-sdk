// Dispatches to either the mock-mode smoke test (Smoke.Run) or the
// state-aware conformance test (Conformance.Run). The choice is driven by
// the MODE env var which run.sh sets per mode:
//   MODE=mock         (default) → Smoke.Run() — 6-method smoke test against Prism
//   MODE=conformance            → Conformance.Run() — 17-scenario state-aware test against the real API
if (Environment.GetEnvironmentVariable("MODE") == "conformance")
{
    Conformance.Run();
    return;
}
Smoke.Run();
