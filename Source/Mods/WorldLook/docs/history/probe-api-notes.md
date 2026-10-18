# Installed API checks (GraphicsProbe)

The original checks below were performed for 0.1.0. The 0.1.2 repair additionally
inspected the installed `UnityEngine.Camera.main` getter with ILSpy; it is used only
as one tagged-enabled hint, never a global camera inventory or gameplay-camera proof.
Compilation against the installed interop also established that
`GetComponents<Component>()` returns `Il2CppArrayBase<Component>`, while the scene-root
array uses `Il2CppReferenceArray<GameObject>`. The resumable adapter retains those
bounded-scope arrays only during an active capture and clears them on release.
These are local signature/compile checks, not execution or performance verification.

These checks used `C:/Users/pc/.dotnet/tools/ilspycmd.exe -t <type> <installed-dll>` against the current game's `MelonLoader/Il2CppAssemblies`, not an assumed upstream Unity package. Decompiled research files stayed in Hermes scratch, not in mod assets or the repository. The Release build then compiled every used signature against those same live interop assemblies. No game process was started.

- `UnityEngine.SceneManagement.SceneManager`: `sceneCount`, `GetSceneAt(int)`.
- `UnityEngine.SceneManagement.Scene`: `IsValid()`, `isLoaded`, `name`, `handle`, `buildIndex`, `GetRootGameObjects()`.
- `UnityEngine.Transform`: `childCount`, `GetChild(int)`, `parent`, `GetSiblingIndex()`; `GameObject.GetComponents<Component>()` includes components on inactive objects without a global search.
- `UnityEngine.Camera`: enabled/depth/clearFlags/cullingMask/HDR/MSAA/depthTextureMode, projection/pixel descriptors and existing `targetTexture`. RenderTexture getters and `IsCreated()` observe an existing texture; nothing is allocated or set.
- `UnityEngine.Renderer`: `sharedMaterials`; `MeshFilter.sharedMesh` and `SkinnedMeshRenderer.sharedMesh`. Mesh metadata getters only, no vertices/indices/pixels.
- `GraphicsSettings.currentRenderPipeline`, `defaultRenderPipeline` and `QualitySettings.renderPipeline` describe pipeline assets. `RenderPipelineManager.s_CurrentPipeline` is exposed as a direct native static-field getter (`il2cpp_field_static_get_value`); only its existing object's native type is observed. No pipeline creation/resolution methods are called.
- `UniversalAdditionalCameraData`: installed public serialized-field proxies `m_CameraType`, `m_RendererIndex`, `m_RequiresDepthTextureOption`, `m_RequiresOpaqueTextureOption`, `m_RequiresDepthTexture`, `m_RequiresColorTexture`, `m_ClearDepth`, `m_RenderShadows`, `m_RenderPostProcessing`, `m_Antialiasing`, `m_VolumeLayerMask`, `m_Cameras`. These field reads deliberately avoid lazy renderer and stack accessors.
- `UniversalRenderPipelineAsset`: installed serialized-field proxies for depth/opaque/HDR/MSAA/render-scale/shadows/batching and `m_RendererDataList` names/types. No scriptable renderer accessor or renderer resolution is used.
- `UnityEngine.Rendering.Volume`: `sharedProfile`, `m_IsGlobal`, priority/weight/blendDistance. `VolumeProfile.components` yields only capped descriptors; lazy instanced profile access is not called.
- Native type identity is obtained with `Il2CppSystem.Object.GetIl2CppType().FullName`. The repository aliases `Il2Cppmscorlib` as `il2cpp`, so the general type-name helper uses `extern alias il2cpp` and accepts `il2cpp::Il2CppSystem.Object` (also covers RenderPipeline, which is not a UnityEngine.Object).
- `EventSystem.current.currentSelectedGameObject`, uGUI `InputField` and TMP `TMP_InputField` are available in the installed UI/TMP assemblies. Manual input checks only the selected object and application focus; it is not a global input-field scan.
- Local Shared source was inspected for `GetUserDataPath`, `SaveTextAtomic` and `ModLogger`. Existing Shared is referenced, never modified or rebuilt by the compile-only mod gate.

Interop signatures and direct field bindings do not prove a native invocation's runtime behavior/performance. Stale getters are caught per field/object/section where possible, but a process-fatal native access violation cannot be made safe by a managed catch. Native integration and frame-time acceptance remain In-Game-Verify open.
