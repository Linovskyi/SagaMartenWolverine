﻿using Domain.PersonAggregate.Enums;
using Shared.Abstractions;

namespace Domain.PersonAggregate.Entities;

public class Payment : Entity
{
    protected Payment()
    {
    }

    public decimal Sum { get; private set; }

    public PaymentTypeEnum PaymentType { get; private set; }

    internal protected static Payment Create(string id, decimal sum, int paymentTypeEnum) => new Payment
    {
        Id = id,
        Sum = sum,
        PaymentType = (PaymentTypeEnum) paymentTypeEnum,
    };
}
