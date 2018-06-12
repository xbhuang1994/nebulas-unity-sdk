'use strict';
BigNumber.config({ ERRORS: false });

var Contract = function () {
    LocalContractStorage.defineProperty(this, "creator", null);
    LocalContractStorage.defineProperty(this, "users", null);
    LocalContractStorage.defineMapProperty(this, "paymentMap");
};

Contract.prototype = {
    init: function () {
        this.creator = Blockchain.transaction.from;
        this.users = {};
    },
    //支付
    payment: function (itemId, limit) {
        let txhash = Blockchain.transaction.hash;
        let value = Blockchain.transaction.value;
        let from = Blockchain.transaction.from;
        if (limit != value) {
            throw new Error("Invalid parameters");
        }
        var payments = this.paymentMap.get(from);
        if (payments == null)
            payments = {};
        payments[txhash] = { "itemId": itemId, "value": value };
        this.paymentMap.set(from, payments);
    },
    //查询用户的付费记录
    queryPayment: function () {
        let from = Blockchain.transaction.from;
        var payments = this.paymentMap.get(from);
        return payments;
    },
    //记录分数
    record: function (nickname, score) {
        this._positiveIntCheck(score);
        this._nickNameCheck(nickname);
        let from = Blockchain.transaction.from;
        var users = this.users;
        users[from] = [from, nickname, score];
        this.users = users;
        return users[from];
    },
    //正整数的检验
    _positiveIntCheck(num) {
        let regex = /^[1-9]\d*$/;
        if (!regex.test(num))
            throw new Error("Invalid Integer: " + num);
    },
    //昵称检验,不含特殊字符的数字、字母、汉字 2-8位
    _nickNameCheck(name) {
        let regex = /^[\u4E00-\u9FA5A-Za-z0-9]{1,8}$/
        if (!regex.test(name))
            throw new Error("Invalid Nick Name: " + name);
    },
    _auth() {
        if (this.creator != from)
            throw new Error("Insufficient permissions");
    },
    //清空排行榜
    clear: function () {
        this._auth();
        this.users = {};
    },
    //查询排行榜所有记录
    queryAll: function () {
        let users = this.users;
        let userArr = Object.keys(users).map(function (key) {
            return users[key];
        });
        userArr.sort((a, b) => {
            return b[2] - a[2];
        });
        return userArr;
    },

    //获取当前用户的记录
    queryOwn: function () {
        var from = Blockchain.transaction.from;
        if (from in this.users) {
            return this.users[from];
        } else {
            throw new Error("Can't find user: " + from);
        }
    },
    takeout: function (amount) {
        this._mustCreator();
        var from = Blockchain.transaction.from;
        let value = new BigNumber(amount);
        var result = this._transfer(from, value);
        if (!result) {
            throw new Error("transfer failed.");
        }
    },
    _mustCreator: function () {
        if (!this._isCreator()) {
            throw new Error("Insufficient permissions.");
        }

    },
    _isCreator: function () {
        return Blockchain.transaction.from == this.creator;
    }
    ,
    _transfer: function (address, value) {
        var result = Blockchain.transfer(address, value);
        console.log('_transfer', address, value, result);
        Event.Trigger("transfer", {
            Transfer: {
                from: Blockchain.transaction.to,
                to: address,
                value: value
            }
        });
        return result;
    },

};
module.exports = Contract;
