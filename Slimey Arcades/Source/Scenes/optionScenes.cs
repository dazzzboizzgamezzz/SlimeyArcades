using Microsoft.Xna.Framework;
using Slimey_Arcades.Objects;

namespace Slimey_Arcades.Scenes
{
    public class OptionScene : Scene
    {
        private const int BackgroundLayer = 9;
        private const int PanelLayer = 8;
        private const int BorderLayer = 7;
        private const int ButtonLayer = 6;
        private const int TextLayer = 5;

        private int volumeLevel = 2;
        private bool gridVisible = true;

        private Polygon[] volumeBlocks;

        private Button yesButton;
        private Button noButton;

        public OptionScene() : base(Color.CornflowerBlue)
        {
        }

        public override void Load()
        {
            Sprite.LayerData.LayerIndex = BackgroundLayer;

            // 中央灰色パネル
            AddRectangle(
                185,
                65,
                910,
                550,
                new Color(190, 190, 190),
                PanelLayer
            );

            CreateVolumeArea();
            CreateControlArea();
            CreateGridArea();
            CreateColorArea();
            CreateBottomButtons();
        }

        // =====================================================
        // ① 音量
        // =====================================================

        private void CreateVolumeArea()
        {

            AddText(
                300,
                120,
                "音量:",
                Color.Black,
                0.18f
            );

            Button leftButton = CreateTextButton(
                500,
                105,
                40,
                40,
                "◀",
                Color.Transparent,
                Color.Black,
                false
            );

            leftButton.Function = DecreaseVolume;

            volumeBlocks = new Polygon[4];

            for (int i = 0; i < volumeBlocks.Length; i++)
            {
                int x = 550 + i * 90;

                AddRectangle(
                    x,
                    110,
                    92,
                    32,
                    Color.Black,
                    BorderLayer
                );

                volumeBlocks[i] = AddRectangle(
                    x + 1,
                    111,
                    90,
                    30,
                    new Color(195, 195, 195),
                    ButtonLayer
                );
            }

            Button rightButton = CreateTextButton(
                910,
                105,
                40,
                40,
                "▶",
                Color.Transparent,
                Color.Black,
                false
            );

            rightButton.Function = IncreaseVolume;

            UpdateVolumeDisplay();
        }

        private void IncreaseVolume()
        {
            if (volumeLevel < 4)
            {
                volumeLevel++;
                UpdateVolumeDisplay();
            }
        }

        private void DecreaseVolume()
        {
            if (volumeLevel > 0)
            {
                volumeLevel--;
                UpdateVolumeDisplay();
            }
        }

        private void UpdateVolumeDisplay()
        {
            for (int i = 0; i < volumeBlocks.Length; i++)
            {
                if (i < volumeLevel)
                {
                    volumeBlocks[i].Sprite.Color =
                        new Color(155, 155, 155);
                }
                else
                {
                    volumeBlocks[i].Sprite.Color =
                        new Color(195, 195, 195);
                }
            }
        }

        // =====================================================
        // ② コントロール
        // =====================================================

        private void CreateControlArea()
        {


            AddText(
                250,
                175,
                "コントロール:",
                Color.Black,
                0.18f
            );

            Button controlButton = CreateBorderedButton(
                460,
                168,
                185,
                34,
                "コントロール変更",
                new Color(195, 195, 195),
                Color.Black
            );

            controlButton.Function = ChangeControl;
        }

        private void ChangeControl()
        {
            System.Console.WriteLine(
                "コントロール変更が押されました"
            );
        }

        // =====================================================
        // ③ グリッド表示
        // =====================================================

        private void CreateGridArea()
        {


            AddText(
                250,
                230,
                "グリッド表示:",
                Color.Black,
                0.18f
            );

            yesButton = CreateBorderedButton(
                460,
                222,
                90,
                34,
                "はい",
                new Color(195, 195, 195),
                Color.Black
            );

            yesButton.Function = () =>
            {
                gridVisible = true;
                UpdateGridDisplay();
            };

            noButton = CreateBorderedButton(
                640,
                222,
                90,
                34,
                "いいえ",
                new Color(195, 195, 195),
                Color.Black
            );

            noButton.Function = () =>
            {
                gridVisible = false;
                UpdateGridDisplay();
            };

            UpdateGridDisplay();
        }

        private void UpdateGridDisplay()
        {
            yesButton.Sprite.Color =
                gridVisible
                    ? new Color(165, 165, 165)
                    : new Color(195, 195, 195);

            noButton.Sprite.Color =
                gridVisible
                    ? new Color(195, 195, 195)
                    : new Color(165, 165, 165);
        }

        // =====================================================
        // ④ 色設定
        // =====================================================

        private void CreateColorArea()
        {


            AddText(
                290,
                285,
                "色設定:",
                Color.Black,
                0.18f
            );

            Button colorButton = CreateBorderedButton(
                460,
                278,
                185,
                34,
                "色設定を変更",
                new Color(195, 195, 195),
                Color.Black
            );

            colorButton.Function = ChangeColor;
        }

        private void ChangeColor()
        {
            System.Console.WriteLine(
                "色設定変更が押されました"
            );
        }

        // =====================================================
        // ⑤ ⑥ 下部ボタン
        // =====================================================

        private void CreateBottomButtons()
        {


            Button resetButton = CreateBorderedButton(
                275,
                400,
                185,
                65,
                "初期値に戻す",
                new Color(145, 145, 145),
                Color.White
            );

            resetButton.Function = ResetSettings;

            Button decideButton = CreateBorderedButton(
                730,
                400,
                280,
                65,
                "決定して、タイトル画面へ",
                new Color(145, 145, 145),
                Color.White
            );

            decideButton.Function = ReturnToTitle;
        }

        private void ResetSettings()
        {
            volumeLevel = 2;
            gridVisible = true;

            UpdateVolumeDisplay();
            UpdateGridDisplay();
        }

        private void ReturnToTitle()
        {
            NextScene = new TitleScene();
        }

        // =====================================================
        // 共通部品
        // =====================================================

        private Polygon AddRectangle(
            int x,
            int y,
            int width,
            int height,
            Color color,
            int layerIndex)
        {
            Polygon rectangle = new Polygon(
                new Transform(x, y, width, height),
                color
            );

            rectangle.Sprite.LayerData.LayerIndex =
                layerIndex;

            Container.ObjectsToLoad.Add(rectangle);

            return rectangle;
        }

        private Text AddText(
            int x,
            int y,
            string text,
            Color color,
            float scale)
        {
            Text newText = new Text(
                new Transform(x, y, 0, 0),
                text,
                scale
            );

            newText.Color = color;

            newText.LayerData.LayerIndex =
                TextLayer;

            Container.ObjectsToLoad.Add(newText);

            return newText;
        }

        private Button CreateTextButton(
           int x,
           int y,
           int width,
           int height,
           string text,
           Color backgroundColor,
           Color textColor,
           bool centered)
        {
            Button button = new Button(
                new Transform(x, y, width, height),
                Shapes.Square,
                backgroundColor,
                text,
                centered
            );

            button.Sprite.LayerData.LayerIndex = ButtonLayer;

            button.TextColor = textColor;
            button.Highlightable = false;
            button.Function = () => { };

            Container.ObjectsToLoad.Add(button);

            return button;
        }

        private Button CreateBorderedButton(
            int x,
            int y,
            int width,
            int height,
            string text,
            Color backgroundColor,
            Color textColor)
        {
            AddRectangle(
                x,
                y,
                width,
                height,
                Color.Black,
                BorderLayer
            );

            Button button = new Button(
                new Transform(
                    x + 2,
                    y + 2,
                    width - 4,
                    height - 4
                ),
                Shapes.Square,
                backgroundColor,
                text,
                true
            );

            button.Sprite.LayerData.LayerIndex = ButtonLayer;

            button.TextColor = textColor;
            button.Function = () => { };

            Container.ObjectsToLoad.Add(button);

            return button;
        }

        private void SetButtonTextLayer(Button button)
        {
            foreach (object child in button.Container.ObjectsToLoad)
            {
                if (child is Text text)
                {
                    text.LayerData.LayerIndex =
                        TextLayer;
                }
            }
        }
    }
}