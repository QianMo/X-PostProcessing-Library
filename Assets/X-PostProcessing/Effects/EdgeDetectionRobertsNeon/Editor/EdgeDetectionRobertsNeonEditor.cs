
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
    [PostProcessEditor(typeof(EdgeDetectionRobertsNeon))]
    public sealed class EdgeDetectionRobertsNeonEditor : PostProcessEffectEditor<EdgeDetectionRobertsNeon>
    {

        SerializedParameterOverride EdgeWidth;
        SerializedParameterOverride Brigtness;
        SerializedParameterOverride BackgroundFade;
        SerializedParameterOverride BackgroundColor;

        public override void OnEnable()
        {
            EdgeWidth = FindParameterOverride(x => x.EdgeWidth);
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

            EditorUtilities.DrawHeaderLabel("Background Property");
            PropertyField(BackgroundFade);
            PropertyField(BackgroundColor);

            EditorUtilities.DrawHeaderLabel("Brigtness");
            PropertyField(Brigtness);

        }

    }
}
        
