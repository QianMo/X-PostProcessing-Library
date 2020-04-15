
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [PostProcessEditor(typeof(EdgeDetectionSobelNeonV2))]
    public sealed class EdgeDetectionSobelNeonV2Editor : PostProcessEffectEditor<EdgeDetectionSobelNeonV2>
    {

        SerializedParameterOverride EdgeWidth;
        SerializedParameterOverride EdgeNeonFade;
        SerializedParameterOverride Brigtness;
        SerializedParameterOverride BackgroundFade;
        SerializedParameterOverride BackgroundColor;
        
        public override void OnEnable()
        {
            EdgeWidth = FindParameterOverride(x => x.EdgeWidth);
            EdgeNeonFade = FindParameterOverride(x => x.EdgeNeonFade);
            Brigtness = FindParameterOverride(x => x.Brigtness);
            BackgroundFade = FindParameterOverride(x => x.BackgroundFade);
            BackgroundColor = FindParameterOverride(x => x.BackgroundColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Edge Property");
            PropertyField(EdgeWidth);
            PropertyField(EdgeNeonFade);


            EditorUtilities.DrawHeaderLabel("Background Property( For Edge Neon Fade <1 )");
            PropertyField(BackgroundFade);
            PropertyField(BackgroundColor);

            EditorUtilities.DrawHeaderLabel("Edge Property");
            PropertyField(Brigtness);

        }
    }
}
        
