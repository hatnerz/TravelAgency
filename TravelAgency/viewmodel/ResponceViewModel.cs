using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class ResponceViewModel
    {
        public Responce CurrentResponce;
        public byte Score
        {
            get => CurrentResponce.Score;
            set => CurrentResponce.Score = value;
        }
        public string Comment
        {
            get => CurrentResponce.Comment;
            set => CurrentResponce.Comment = value;
        }
        public ResponceViewModel(Responce currentResponce)
        {
            CurrentResponce = currentResponce;
        }
    }
}
